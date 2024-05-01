// Get current date and time
const currentDate = new Date();
const hours = currentDate.getHours();

// Determine the greeting based on the time of day
let greeting;
if (hours < 12) {
    greeting = 'Good morning!';
} else if (hours < 18) {
    greeting = 'Good afternoon!';
} else {
    greeting = 'Good evening!';
}

// Display the greeting on the page
const greetingElement = document.getElementById('greeting');
greetingElement.textContent = greeting;