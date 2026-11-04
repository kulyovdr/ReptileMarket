window.ReptileMap = (() => {
    let map = null;
    let marker = null;
    let ref = null;
    let seq = 0;

    function formatAddress(res) {
        const a = (res && res.address) || {};
        const city = a.city || a.town || a.village || a.municipality || a.county || "";
        const road = a.road || a.pedestrian || a.footway || a.cycleway || "";
        const house = a.house_number || "";
        const area = a.suburb || a.neighbourhood || a.quarter || a.city_district || a.hamlet || "";
        let detail = "";
        if (road) {
            detail = road + (house ? ", " + house : "");
        } else if (area) {
            detail = area;
        }
        const locality = city || a.state || "";
        let text = locality && detail ? locality + ", " + detail : (locality || detail);
        if (!text) {
            text = ((res && res.display_name) || "").replace(/^Россия,\s*/i, "");
            text = text.split(",").slice(0, 4).join(",").trim();
        }
        return text;
    }

    async function reverse(lat, lon) {
        try {
            const response = await fetch(
                "https://nominatim.openstreetmap.org/reverse?format=jsonv2&accept-language=ru&zoom=18&lat=" + lat + "&lon=" + lon);
            const data = await response.json();
            const text = formatAddress(data);
            if (text) {
                return text;
            }
        } catch (e) {
        }
        return lat.toFixed(5) + ", " + lon.toFixed(5);
    }

    function placeMarker(lat, lon) {
        if (marker) {
            marker.setLatLng([lat, lon]);
        } else {
            marker = L.marker([lat, lon], { draggable: true }).addTo(map);
            marker.on("dragend", () => {
                const p = marker.getLatLng();
                handlePick(p.lat, p.lng);
            });
        }
    }

    async function handlePick(lat, lon) {
        placeMarker(lat, lon);
        const address = await reverse(lat, lon);
        const current = seq;
        if (ref && current === seq) {
            await ref.invokeMethodAsync("OnPicked", address, lat, lon);
        }
    }

    return {
        open(dotnetRef, elementId, lat, lon) {
            this.close();
            seq++;
            ref = dotnetRef;
            const hasPoint = lat !== 0 && lon !== 0;
            map = L.map(elementId, { center: hasPoint ? [lat, lon] : [55.76, 37.62], zoom: hasPoint ? 17 : 10 });
            L.tileLayer("https://tile.openstreetmap.org/{z}/{x}/{y}.png", {
                maxZoom: 19,
                attribution: "&copy; OpenStreetMap contributors"
            }).addTo(map);
            map.on("click", (e) => handlePick(e.latlng.lat, e.latlng.lng));
            setTimeout(() => {
                if (map) {
                    map.invalidateSize();
                }
            }, 60);
        },

        async search(query) {
            if (!map || !query || !query.trim()) {
                return null;
            }
            try {
                const url = "https://nominatim.openstreetmap.org/search?format=jsonv2&accept-language=ru&limit=1"
                    + "&bounded=1&viewbox=19,82,190,41&q=" + encodeURIComponent(query.trim());
                const response = await fetch(url);
                const results = await response.json();
                if (!results || !results.length) {
                    return null;
                }
                const item = results[0];
                const lat = parseFloat(item.lat);
                const lon = parseFloat(item.lon);
                if (item.boundingbox) {
                    const b = item.boundingbox.map(parseFloat);
                    map.fitBounds([[b[0], b[2]], [b[1], b[3]]], { maxZoom: 16 });
                } else {
                    map.setView([lat, lon], 17);
                }
                placeMarker(lat, lon);
                const text = formatAddress(item) || (item.display_name || "").replace(/^Россия,\s*/i, "");
                const parts = text.split(",").slice(0, 5).join(",").trim();
                return parts || (lat.toFixed(5) + ", " + lon.toFixed(5));
            } catch (e) {
                return null;
            }
        },

        close() {
            seq++;
            if (map) {
                map.remove();
                map = null;
                marker = null;
            }
            ref = null;
        }
    };
})();
