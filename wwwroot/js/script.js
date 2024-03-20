const hamBurger = document.querySelector(".toggle-btn");

hamBurger.addEventListener("click", function () {
    document.querySelector("#sidebar").classList.toggle("expand");
});

function registroExito(message) {
    Swal.fire({
        icon: "success",
        title: message,
        showConfirmButton: false,
        timer: 1500
    });
}
function registroError(message) {
    Swal.fire({
        icon: "error",
        title: "Error",
        text: message,
        showConfirmButton: false,
        timer: 1500
    });
}

