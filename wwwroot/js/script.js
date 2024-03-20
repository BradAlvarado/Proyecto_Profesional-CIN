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


