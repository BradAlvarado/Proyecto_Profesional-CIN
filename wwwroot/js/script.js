const hamBurger = document.querySelector(".toggle-btn");
// EVENTO PARA EL NAVBAR
hamBurger.addEventListener("click", function () {
    document.querySelector("#sidebar").classList.toggle("expand");
});


// Alerta de Success
function registroExito(message) {
    Swal.fire({
        icon: "success",
        title: message,
        showConfirmButton: false,
        timer: 2000
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
