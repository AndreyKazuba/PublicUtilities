const jwtkey = 'public-utilities-jwt';

const loginButton = document.getElementById('login-button');
loginButton.onclick = async e => {
    const response = await fetch("/api/login", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            login: document.getElementById("login").value,
            password: document.getElementById("password").value,
        })
    });

    if (response.status === 200) {
        localStorage.setItem(jwtkey, await response.json())
        location.href = '../admin/admin.html';
    }

    if (response.status === 401) {
        const error = document.getElementById('error-message');
        error.style.display = 'flex';
    }
};