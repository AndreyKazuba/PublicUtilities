const jwtkey = 'public-utilities-jwt';

const updateButtonsState = () => {
    const jwt = localStorage.getItem(jwtkey)

    if (jwt) {
        document.getElementById('log-in-button').style.display = 'none';
        document.getElementById('log-out-button').style.display = 'block';
        document.getElementById('applications-button').style.display = 'block';
    }
    else {
        document.getElementById('log-in-button').style.display = 'block';
        document.getElementById('log-out-button').style.display = 'none';
        document.getElementById('applications-button').style.display = 'none';
    }
};

updateButtonsState();

const logOutButton = document.getElementById('log-out-button');
if (logOutButton) {
    logOutButton.onclick = () => {
        localStorage.clear();
        updateButtonsState();
    };
}

const xhr = new XMLHttpRequest();
xhr.open("GET", "/api/approvedApplications");
xhr.onreadystatechange = () => {
    if (xhr.readyState == 4) {
        if (xhr.status == 200) {
            const result = JSON.parse(xhr.response);
            for (const application of result) {
                insertApplicationItem(application);
            }
        }
    }
};
xhr.send();

const main = document.getElementById('main');
const insertApplicationItem = application => {
    const app = document.createElement('div');
    app.classList.add('application');

    const header = document.createElement('div');
    header.classList.add('application-header');

    const headerContent = document.createElement('span');
    headerContent.innerText = 'Заявка ';
    const headerNumber = document.createElement('span');
    headerNumber.innerText = `#${application.id}`;

    header.appendChild(headerContent);
    header.appendChild(headerNumber);

    const body = document.createElement('div');
    body.classList.add('application-body');

    const applicantNameRecord = createRecord('Заявитель', application.applicantName);
    const addressRecord = createRecord('Адрес', application.address);
    const typeOfWorkRecord = createRecord('Род работ', application.typeOfWork);
    const scaleOfWorkRecord = createRecord('Масштаб', application.scaleOfWork);
    const dateOfWorkRecord = createRecord('Дата выполнения', new Date(application.dateOfWork).toLocaleDateString('ru-RU'));
    const workersRecord = createRecord('Испольнители', application.workers);

    body.appendChild(applicantNameRecord);
    body.appendChild(addressRecord);
    body.appendChild(typeOfWorkRecord);
    body.appendChild(scaleOfWorkRecord);
    body.appendChild(dateOfWorkRecord);
    body.appendChild(workersRecord);

    app.appendChild(header);
    app.appendChild(body);

    main.appendChild(app);
};

const createRecord = (key, value) => {
    const record = document.createElement('p');
    record.classList.add('record');

    const keyElement = document.createElement('span');
    keyElement.classList.add('key');
    keyElement.innerText = `${key}: `;

    const valueElement = document.createElement('span');
    valueElement.innerText = value;

    record.appendChild(keyElement);
    record.appendChild(valueElement);
    
    return record;
};