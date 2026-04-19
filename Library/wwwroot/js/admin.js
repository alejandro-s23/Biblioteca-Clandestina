function openDetails(id) {
    const modalElement = document.getElementById(id);
    if (modalElement) {
        // Inicializa e abre o modal do Bootstrap
        const myModal = new bootstrap.Modal(modalElement);
        myModal.show();
    }
}

function closeDetails(id) {
    const modalElement = document.getElementById(id);
    const modalInstance = bootstrap.Modal.getInstance(modalElement);
    if (modalInstance) {
        modalInstance.hide();
        $('.modal-backdrop').remove();
    }
}

document.querySelectorAll('button[data-bs-toggle="tab"]').forEach(tab => {
    tab.addEventListener('shown.bs.tab', function (event) {
        const target = event.target.getAttribute("data-bs-target");
        const url = `/Admin/Relatorios/Get${target.replace('#', '')}`;

        // Busca o conteúdo apenas se estiver vazio
        if ($(target).is(':empty')) {
            $(target).load(url);
        }
    });
});
