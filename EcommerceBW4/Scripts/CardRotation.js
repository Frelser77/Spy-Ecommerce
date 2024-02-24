/*document.addEventListener('DOMContentLoaded', function () {
    var flipButton = document.querySelector('.btn-pstn');
    var card = document.querySelector('.con');
    var loginButton = document.querySelector('.log-in');
    var forgotPassword = document.querySelector('.frgt-pass');
    var signUpButton = document.querySelector('.sign-up');
    var etaTextBox = document.getElementById('<%= txtEta.ClientID %>');

    flipButton.addEventListener('click', function () {
        card.classList.toggle('card-flipped');
        etaTextBox.style.display = etaTextBox.style.display === 'none' ? 'block' : 'none';
        loginButton.innerText = loginButton.innerText === 'Log In' ? 'Sign Up' : 'Log In';
        forgotPassword.style.display = 'none';
        signUpButton.style.display = 'block';
    });
});
*/

/*
document.addEventListener('DOMContentLoaded', function () {
    var flipButton = document.querySelector('.btn-pstn');
    var card = document.querySelector('.con'); // Assicurati che questo sia il container della card
    var loginButton = document.querySelector('.log-in');
    var forgotPassword = document.querySelector('.frgt-pass');
    var signUpButton = document.querySelector('.sign-up');
    var etaTextBox = document.getElementById('<%= txtEta.ClientID %>');

    flipButton.addEventListener('click', function () {
        card.classList.toggle('flipped'); // Aggiungi/rimuovi la classe per la rotazione
        etaTextBox.style.display = etaTextBox.style.display === 'none' ? 'block' : 'none';
        loginButton.innerText = loginButton.innerText === 'Log In' ? 'Sign Up' : 'Log In';
        forgotPassword.style.display = 'none';
        signUpButton.style.display = 'block';
    });
});
*/

/*
var flipButton = document.querySelector('.btn-pstn');
var card = document.querySelector('.con'); // Assicurati che questo sia il container della card
var cardBack = document.querySelector('.card__back'); // Seleziona il retro della card
var cardFront = document.querySelector('.card__front');
var loginButton = document.querySelector('.log-in');
var forgotPassword = document.querySelector('.frgt-pass');
var signUpButton = document.querySelector('.sign-up');
var etaTextBox = document.getElementById('<%= txtEta.ClientID %>');
var btn1 = document.getElementById('btn-1');
var btn2 = document.getElementById('btn-2');



flipButton.addEventListener('click', function () {
    card.classList.toggle('flipped'); // Aggiungi/rimuovi la classe per la rotazione

    // Cambia il display del retro della card in base allo stato flipped
    cardBack.style.display = card.classList.contains('flipped') ? 'block' : 'none';
cardFront.style.display = card.classList.contains('flipped') ? 'none' : 'block';

    // Gestisci la visualizzazione degli elementi del form in base allo stato della card
    if (card.classList.contains('flipped')) {
        btn1.style.display = 'none';
        btn2.style.display = 'block';
        loginButton.style.display = 'none';
        forgotPassword.style.display = 'none';
        signUpButton.style.display = 'block';
        etaTextBox.style.display = 'block';
    } else {
        btn1.style.display = 'block';
        btn2.style.display = 'none';
        loginButton.style.display = 'block';
        forgotPassword.style.display = 'block';
        signUpButton.style.display = 'none';
        etaTextBox.style.display = 'none';
    }
});

*/
function flipCard() {
    var card = document.querySelector('.flip');
    var cardBack = document.querySelector('.card__back');
    var cardFront = document.querySelector('.card__front');
    var loginButton = document.querySelector('.log-in');
    var forgotPassword = document.querySelector('.frgt-pass');
    var signUpButton = document.querySelector('.sign-up');
    var btn1 = document.getElementById('btn-1');
    var btn2 = document.getElementById('btn-2');
    var etaTextBox = document.getElementById('<%= txtEta.ClientID %>'); // Usa ClientID per controlli ASP.NET

    card.classList.toggle('flipped');

    // Controlla lo stato della classe 'flipped' e aggiusta la visibilità delle componenti
    if (card.classList.contains('flipped')) {
        cardFront.style.display = 'none';
        cardBack.style.display = 'block';
        btn1.style.display = 'none'; // Nasconde la freccia sul fronte quando la card è flippata
        btn2.style.display = 'block'; // Mostra la freccia sul retro quando la card è flippata
        loginButton.style.display = 'none';
        forgotPassword.style.display = 'none';
        signUpButton.style.display = 'block';
        etaTextBox.style.display = 'block';
    } else {
        cardFront.style.display = 'block';
        cardBack.style.display = 'none';
        btn1.style.display = 'block'; // Mostra la freccia sul fronte quando la card non è flippata
        btn2.style.display = 'none'; // Nasconde la freccia sul retro quando la card non è flippata
        loginButton.style.display = 'block';
        forgotPassword.style.display = 'block';
        signUpButton.style.display = 'none';
        etaTextBox.style.display = 'none';
    }
}
