const lcontainer = document.getElementById('lcontainer');
const registerBtn = document.getElementById('register');
const loginBtn = document.getElementById('login');

registerBtn.addEventListener('click', () => {
    lcontainer.classList.add("active");
});

loginBtn.addEventListener('click', () => {
    lcontainer.classList.remove("active");
});