
var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl);
});
document.addEventListener('DOMContentLoaded', function () {
    const statusAlert = document.querySelector('#status-message-alert');

    if (statusAlert) {
        setTimeout(() => {
            const alertInstance = new bootstrap.Alert(statusAlert);
            alertInstance.close();
        }, 5000);
    }
});