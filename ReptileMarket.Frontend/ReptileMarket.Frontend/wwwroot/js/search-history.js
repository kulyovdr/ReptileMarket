const KEY = 'rm-sear…tory';

window.rmSearchHistory = {
    get: () => {
        try {
            return JSON.parse(localStorage.getItem(KEY)) || [];
        } catch {
            return [];
        }
    },
    add: (q) => {
        if (!q) {
            return;
        }
        let items = window.rmSearchHistory.get().filter(x => x.toLowerCase() !== q.toLowerCase());
        items.unshift(q);
        try {
            localStorage.setItem(KEY, JSON.stringify(items.slice(0, 8)));
        } catch {
        }
    },
    clear: () => localStorage.removeItem(KEY)
};
