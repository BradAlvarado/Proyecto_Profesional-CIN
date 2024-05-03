var currentPath = window.location.pathname;

        //Busca los elementos de enlace en tu menú de navegación
var navLinks = document.querySelectorAll('.sidebar-link');


        //Itera sobre los enlaces y verifica si la URL coincide
navLinks.forEach(function (link) {
    if (link.getAttribute('href') === currentPath) {
        link.parentElement.classList.add('bg-secondary', 'border-2', 'border-start', 'border-white'); //Agrega las clases
    }
});

// Selecciona el botón hamBurger
const hamBurger = document.querySelector(".toggle-btn");

// Selecciona el sidebar
const sidebar = document.querySelector("#sidebar");

// Función para ajustar el estado del sidebar y del botón hamBurger
function adjustSidebar() {
    if (window.innerWidth <= 800) {
        sidebar.classList.add("expand");
        hamBurger.disabled = true;
    } else {
        sidebar.classList.remove("expand");
        hamBurger.disabled = false;
    }
}

// Función para toggle el estado del sidebar cuando se hace clic en el botón hamBurger
hamBurger.addEventListener("click", function () {
    sidebar.classList.toggle("expand");
});

// Llama a la función para ajustar el estado del sidebar al cargar la página y al cambiar el tamaño de la ventana
window.onload = adjustSidebar;
window.onresize = adjustSidebar;

// Alerta de Success
function registroExito(message) {
    Swal.fire({
        icon: "success",
        title: message,
        showConfirmButton: false,
        timer: 1500
    });
}

function alertaError(message) {
    Swal.fire({
        icon: 'error',
        title: 'Error',
        text: message,
        confirmButtonColor: '#3085d6',
        timer: 2000,
        timerProgressBar: true
    });
}


// Funcion para dar formato a la cedula
function formatCedula(input) {
    // Elimina todos los caracteres que no sean números
    var cedula = input.value.replace(/\D/g, '');

    // Agrega el formato deseado
    if (cedula.length > 4) {
        cedula = cedula.substring(0, 1) + '-' + cedula.substring(1, 5) + '' + cedula.substring(5, 9);
    } else if (cedula.length > 1) {
        cedula = cedula.substring(0, 1) + '-' + cedula.substring(1);
    }

    // Actualiza el valor del input
    input.value = cedula;
}

function formatTelefono(input) {
    // Elimina todos los caracteres que no sean números
    var telefono = input.value.replace(/\D/g, '');

    // Agrega el formato deseado
    if (telefono.length > 4) {
        telefono = telefono.substring(0, 4) + '-' + telefono.substring(4, 8);
    }

    // Actualiza el valor del input
    input.value = telefono;
}

//Calculamos la edad auto al ingresar la fecha de nacimiento
function calcularEdad(fechaNacimientoInputId, edadInputId) {
    var fechaNacimientoInput = document.getElementById(fechaNacimientoInputId);
    var edadInput = document.getElementById(edadInputId);

    fechaNacimientoInput.addEventListener('change', function () {
        var edad = 0;
        var fechaNacimiento = new Date(this.value);
        var fechaActual = new Date();
        edad = fechaActual.getFullYear() - fechaNacimiento.getFullYear();
        if (fechaNacimiento.getMonth() > fechaActual.getMonth() || (fechaNacimiento.getMonth() === fechaActual.getMonth() && fechaNacimiento.getDate() > fechaActual.getDate())) {
            edad--;
        }
        edadInput.value = edad;
    });
}


// Funcion SweetAlert para confirmar Eliminar
function confirmDelete(id, nombre, controller) {
    Swal.fire({
        title: `Deseas eliminar el registro de ${nombre}? `,
        text: "No podras revertir los cambios!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: 'Cancelar',
        confirmButtonText: 'Si, eliminarlo!'
    }).then((result) => {
        if (result.isConfirmed) {
            // Si el usuario confirma, envía la solicitud de eliminación con AJAX
            $.ajax({
                url: `/${controller}/Delete`,
                type: 'POST',
                data: { id: id },
                dataType: 'json',
                success: function (response) {
                    
                    location.reload();
                },
                error: function () {
                    // Errores de la solicitud AJAX 
                    alertaError(`Registro ${id} no fue encontrado.`)
                    
                }
            });
        }
    })
}

