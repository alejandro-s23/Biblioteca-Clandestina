// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const input = document.getElementById('authorInput');
const dropdown = document.getElementById('authorDropdown');
const items = document.querySelectorAll('.dropdown-item-manuscrito');

// Mostrar lista ao focar
input.addEventListener('focus', () => dropdown.classList.remove('d-none'));

// Filtrar enquanto digita
input.addEventListener('input', (e) => {
    const val = e.target.value.toLowerCase();
    items.forEach(item => {
        const text = item.textContent.toLowerCase();
        item.style.display = text.includes(val) ? 'block' : 'none';
    });
});

// Selecionar item ao clicar
items.forEach(item => {
    item.addEventListener('click', () => {
        input.value = item.textContent;
        dropdown.classList.add('d-none');
    });
});

// Fechar ao clicar fora
document.addEventListener('click', (e) => {
    if (!input.contains(e.target) && !dropdown.contains(e.target)) {
        dropdown.classList.add('d-none');
    }
});

//Modals Globais
document.addEventListener("DOMContentLoaded", () => {
    let myErrorModal = new bootstrap.Modal(document.getElementById('globalModalError'));
    show(myErrorModal);
});

function abrirLivro(btn) {
    const titulo = btn.getAttribute('data-titulo');
    const conteudo = btn.getAttribute('data-sinopsia');
    
    document.getElementById('sinopseTitulo').innerText = titulo;
    document.getElementById('sinopseConteudo').innerText = conteudo ? conteudo : "Este manuscrito ainda possui uma sinopsia escrita! \n Solicite à internet seu resumo!";

    const modal = bootstrap.Modal.getOrCreateInstance(document.getElementById('modalSinopse'));
    modal.show();
}