window.rmNavArrows = (inner, leftBtn, rightBtn) => {
    if (!inner || !leftBtn || !rightBtn) {
        return;
    }

    const step = Math.max(inner.clientWidth * 0.75, 160);

    const update = () => {
        const max = inner.scrollWidth - inner.clientWidth;
        leftBtn.classList.toggle("hidden", inner.scrollLeft <= 2);
        rightBtn.classList.toggle("hidden", inner.scrollLeft >= max - 2);
    };

    inner.addEventListener("scroll", update, { passive: true });
    window.addEventListener("resize", update);
    leftBtn.addEventListener("click", () => inner.scrollBy({ left: -step, behavior: "smooth" }));
    rightBtn.addEventListener("click", () => inner.scrollBy({ left: step, behavior: "smooth" }));
    update();
};
