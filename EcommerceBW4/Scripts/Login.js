function show() {
    var p = document.getElementById('txtPassword');
    p.setAttribute('type', 'text');
}

function hide() {
    var p = document.getElementById('txtPassword');
    p.setAttribute('type', 'password');
}

var pwShown = 0;

document.getElementById("eye").addEventListener("click", function () {
    if (pwShown == 0) {
        pwShown = 1;
        show();
    } else {
        pwShown = 0;
        hide();
    }
}, false);

/* funzione mouse hover */
/*
document.addEventListener('DOMContentLoaded', (event) => {
    const card = document.querySelector(".card");
    const overlay = document.querySelector(".overlay");

    // La funzione per applicare l'effetto overlay.
    const applyOverlayMask = (e) => {
        const x = e.pageX - card.offsetLeft;
        const y = e.pageY - card.offsetTop;

        overlay.style = `--opacity: 1; --x: ${x}px; --y:${y}px;`;
    };

    // Ascolta l'evento di movimento del mouse.
    document.body.addEventListener("pointermove", applyOverlayMask);
});
*/

/* FUNZIONE LOADING */
/*


function hideLoader() {
console.log('hideLoader eseguito');
    document.getElementById('loader').style.display = 'none';
}

document.addEventListener('DOMContentLoaded', function () {
    console.log('DOM caricato' + "")
    hideLoader();
});
*/
/*
function showLoader() {
    console.log('showLoader eseguito');
    document.getElementById('loader').style.display = 'flex';
}

document.addEventListener('DOMContentLoaded', function () {
    let loginButton = document.getElementById('<%= LoginButton.ClientID %>');
    console.log('DOM caricato' + "loginbutton preso" + loginButton)
    if (loginButton) {
        loginButton.addEventListener('click', function () {
            document.getElementById('loader').style.display = 'block';
            console.log('click' + "loader mostrato")
        });
    }
})
*/;

document.addEventListener('DOMContentLoaded', function () {
    var loginButton = document.getElementById('<%= LoginButton.ClientID %>');
    if (loginButton) {
        loginButton.addEventListener('click', function () {
            document.getElementById('loader').style.display = 'block';
        });
    }
});