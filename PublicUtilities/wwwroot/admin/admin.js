const jwtkey = 'public-utilities-jwt';
const jwt = localStorage.getItem(jwtkey);
let processingApplicationId;

const modal = document.getElementById('processModal');

document.addEventListener('DOMContentLoaded', async () => {
    await updateTable();
    await updateWorkerList();
});

const updateTable = async () => {
    if (jwt) {
        let response = await fetch('/api/pendingApplications', {
            method: 'GET',
            headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Authorization': `Bearer ${jwt}` },
        });

        if (response.status === 200) {
            const main = document.getElementById('main');
            main.innerHTML = '';
            for (const application of await response.json()) {
                insertApplicationItem(application);
            }
        }        
    }
}

const updateWorkerList = async () => {
    const response = await fetch('/api/workers', {
        method: 'GET',
        headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Authorization': `Bearer ${jwt}` },
    });

    if (response.status === 200) {
        const list = document.getElementById('worker-list');
        list.innerHTML = '';
        for (const worker of await response.json()) {
            setWorker(worker);
        }
    }
};

const createTh = (content) => {
    const th = document.createElement('th');
    th.innerText = content;
    return th;
};

const setWorker = (worker) => {
    const list = document.getElementById('worker-list');
    const workerItem = document.createElement('div');
    workerItem.classList.add('worker-item');

    const input = document.createElement('input');
    input.setAttribute('type', 'checkbox');
    input.setAttribute('data-worker-id', worker.id);

    const p = document.createElement('p');
    p.classList.add('worker-name');
    p.innerText = `${worker.lastName} ${worker.firstName}`;

    workerItem.appendChild(input);
    workerItem.appendChild(p);

    list.appendChild(workerItem);
};

const logOutButton = document.getElementById('log-out-button');
if (logOutButton) {
    logOutButton.onclick = () => {
        localStorage.clear();
        location.href = '../'
    };
}

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

    const confirmButton = document.createElement('a');
    confirmButton.classList.add('button');
    confirmButton.innerText = 'Подтвердить';
    confirmButton.setAttribute('data-app-id', application.id);
    confirmButton.addEventListener('click', openProcessModal);

    body.appendChild(applicantNameRecord);
    body.appendChild(addressRecord);
    body.appendChild(typeOfWorkRecord);
    body.appendChild(scaleOfWorkRecord);
    body.appendChild(dateOfWorkRecord);
    body.appendChild(confirmButton);

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

const openProcessModal = (event) => {
    processingApplicationId = event.target.getAttribute('data-app-id');
    modal.style.display = 'block';
};


document.getElementById('close-modal-button').onclick = () => {
    processingApplicationId = null;
    modal.style.display = 'none';
};

document.getElementById('confirm-processing-button').onclick = async () => {
    const checkedBoxes = document.querySelectorAll('input[type="checkbox"]:checked');
    const workerIds = [];
    for (const checkbox of checkedBoxes.values()) {
        workerIds.push(checkbox.getAttribute('data-worker-id'));
    }

    if (!workerIds.length) {
        document.getElementById('modal-warning').style.display = 'flex';
        return;
    }

    if (jwt) {
        const response = await fetch('/api/approveApplication', {
            method: 'POST',
            headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Authorization': `Bearer ${jwt}` },
            body: JSON.stringify({
                applicationId: processingApplicationId,
                workerIds: workerIds,
            })
        });


        if (response.status === 200) {
            modal.style.display = 'none';
            processingApplicationId = null;
            document.getElementById('modal-warning').style.display = 'none';
            updateTable();
            updateWorkerList();
        }
    }
};

window.onclick = (event) => {
    if (event.target == modal) {
        modal.style.display = 'none';
        processingApplicationId = null;
    }
}

