function MakeUpdateQtyButtonVisible(id, visible) {
    const updateQtyBtn = document.querySelector(`button[data-id="${id}"]`);
    if (!updateQtyBtn) return;
    if (visible) {
        updateQtyBtn.style.display = 'inline-block';
    } else {
        updateQtyBtn.style.display = 'none';
    }
}
