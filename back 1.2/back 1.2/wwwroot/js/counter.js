// Получаем элементы DOM
const counterDisplay = document.getElementById('counter');
const incrementBtn = document.getElementById('incrementBtn');
const resetBtn = document.getElementById('resetBtn');

let count = 0; // Изначальное значение счетчика

// Функция для увеличения счетчика
function incrementCounter() {
    count++;
    counterDisplay.textContent = count;
}

// Функция для сброса счетчика
function resetCounter() {
    count = 0;
    counterDisplay.textContent = count;
}

// Назначаем функции для событий клика на кнопки
incrementBtn.addEventListener('click', incrementCounter);
resetBtn.addEventListener('click', resetCounter);