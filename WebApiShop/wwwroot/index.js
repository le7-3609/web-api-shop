const openNewUser = document.querySelector(".addUser")
const newUserBox = document.querySelector(".newUser")


function toggleNewUser() {
    const newUserDiv = document.querySelector('.newUser');
    if (newUserDiv.style.display === 'flex') {
        newUserDiv.style.display = 'none';
    } else {
        newUserDiv.style.display = 'flex';
    }
}

openNewUser.addEventListener("click", e => {
    openNewUser.style.color = "red"
    newUserBox.style.display = "flex"
})

const getNewUser = () => {
    const user = {
        Email: document.querySelector("#userName").value,
        FirstName: document.querySelector("#firstName").value,
        LastName: document.querySelector("#lastName").value,
        Password: document.querySelector("#password").value
    };
    return user;
}

const getExistUser = () => {
    const user = {
        Email: document.querySelector("#existingUserName").value,
        Password: document.querySelector("#existingUserPassword").value
    };
    return user;
}

async function createUser() {
    console.log("wellcome")
    const newUser = getNewUser()
    try {
        const responsePost = await fetch('api/Users', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',

            },
            body: JSON.stringify(newUser)
        });
        if (!responsePost.ok) {
            throw new Error(`HTTP error! status:${responsePost.status}`)
        }

        const dataPost = await responsePost.json();
        alert(" נרשמת בהצלחה")
    }
    catch (e) {
        console.log(e)
    }
};

async function login() {
    console.log("wellcome back")
    const existUser = getExistUser()
    try {
        const responsePost = await fetch("/api/users/login", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(existUser)
        });
        if (!responsePost.ok) {
            throw new Error(`HTTP error! status:${responsePost.status}`)
        }
        if (responsePost.status == 204) {
            alert("שם משתמש לא קיים")
        }
        else {
            const dataPost = await responsePost.json();
            sessionStorage.setItem("User", JSON.stringify(dataPost))
            window.location.href = "UserDetails.html"
        }
    }
    catch (e) {
        console.log(e)
    }
};