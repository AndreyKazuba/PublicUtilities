const loginButton = document.getElementById('submit-application-button');
loginButton.onclick = async e => {
    const response = await fetch("/api/createApplication", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            name: document.getElementById("applicantName").value,
            address: document.getElementById("address").value,
            dateOfWork: document.getElementById("dateOfWork").value,
            typeOfWork: document.getElementById("typeOfWork").value,
            scaleOfWork: document.getElementById("scaleOfWork").value,
        })
    });

    if (response.status === 200) {
        location.href = '../';
    }

    if (response.status === 400) {
        const error = document.getElementById('error-message');
        error.style.display = 'flex';
    }
};


