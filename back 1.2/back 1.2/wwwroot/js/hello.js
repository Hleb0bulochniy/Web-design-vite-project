// Получаем кнопку и элемент для отображения сообщения
const btn = document.getElementById('btn');
const message = document.getElementById('message');

// Функция, которая будет вызываться при клике на кнопку
function showMessage() {
    message.textContent = 'Привет, мир!';
}

// Назначаем функцию для события клика на кнопке
btn.addEventListener('click', showMessage);