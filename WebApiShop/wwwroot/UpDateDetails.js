const text = document.querySelector(".text")
let oldUser = JSON.parse(sessionStorage.getItem("User"))
const updatedUserOpen = document.querySelector(".open")
const updatedUserBox = document.querySelector(".updatedUser")

text.textContent = `שלום ${oldUser.firstName}`

updatedUserOpen.addEventListener("click", e => {
    updatedUserOpen.style.color = "red"
    updatedUserBox.style.display = "flex"
})

const getDataRfomForm = () => { 
    const updatedUser = {
        Email: document.querySelector("#userName").value || oldUser.email,
        FirstName: document.querySelector("#firstName").value || oldUser.firstName,
        LastName: document.querySelector("#lastName").value || oldUser.lastName,
        Password: document.querySelector("#password").value || oldUser.password,
        UserId: oldUser.userId
    }
    return updatedUser
}

const updateUserDetails = async () => {
    const updatedUser = getDataRfomForm();
    try {
        const responsePost = await fetch(`api/users/${oldUser.userId}`, {
            method: 'Put',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatedUser)

        });
        if (!responsePost.ok) {
            throw new Error(`HTTP error! status:${responsePost.status}`)
        }
        sessionStorage.setItem("User", JSON.stringify(updatedUser))
        alert("הפרטים עודכנו בהצלחה")
    }
    catch (error) {
        console.log(error)
    }
}