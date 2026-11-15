document.addEventListener('click', (e) => {
    const btn = e.target.closest('.pv-heart, .fav-favorite');
    if (!btn) {
        return;
    }
    btn.classList.remove('heart-beat');
    void btn.offsetWidth;
    btn.classList.add('heart-beat');
    btn.addEventListener('animationend', () => btn.classList.remove('heart-beat'), { once: true });
});
