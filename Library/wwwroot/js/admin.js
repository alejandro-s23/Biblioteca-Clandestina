function openDetails(id) {
    let modalElement = document.getElementById(id);
    if (modalElement) {
        // Inicializa e abre o modal do Bootstrap
        let myModal = new bootstrap.Modal(modalElement);
        myModal.show();
    }
}

function closeDetails(id) {
    let element = document.getElementById(id);
    if (element) {
        // Tenta pegar a instância existente que o Bootstrap criou
        let modalInstance = bootstrap.Modal.getInstance(element);

        // Se não encontrar, tenta criar uma apenas para fechar
        if (!modalInstance) {
            modalInstance = new bootstrap.Modal(element);
        }

        modalInstance.hide();

        // Remove o backdrop manualmente caso o Bootstrap falhe em limpar
        $('.modal-backdrop').remove();
        $('body').removeClass('modal-open').css('overflow', '');
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
