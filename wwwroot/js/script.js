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
    if (telefono.length > 3) {
        telefono = '+' + telefono.substring(0, 3) + ' ' + telefono.substring(3, 7) + '-' + telefono.substring(7, 11);
    } else if (telefono.length > 0) {
        telefono = '+' + telefono.substring(0, 3);
    }

    // Actualiza el valor del input
    input.value = telefono;
}

function confirmDelete(id, nombre, controller) {
    Swal.fire({
        title: `¿Deseas eliminar el registro de ${nombre}? `,
        text: "¡No podrás revertir esto!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, eliminarlo!'
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
                    alert('Error al intentar de eliminar el registro '+ id)
                    
                }
            });
        }
    })
}
