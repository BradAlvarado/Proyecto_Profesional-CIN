using System;
using System.Collections.Generic;

namespace Sistema_CIN.Models
{
    public partial class Operaciones
    {
        public Operaciones()
        {
            RolOperacions = new HashSet<RolOperacion>();
        }

        public int IdOp { get; set; }
        public string NombreOp { get; set; } = null!;
        public int? IdModulo { get; set; }

        public virtual Modulo? IdModuloNavigation { get; set; }
        public virtual ICollection<RolOperacion> RolOperacions { get; set; }
    }
}

/*

Ayudame primero a desarrollar el POST Create de mi RolesPermisos Model, el cual contiene las siguientes 4 tablas:

1. Rol: IdRol, NombreRol.
// Ejemplo 
- idRol: 1
  - NombreRol: Docente

2. Modulos: 
// Ejemplo
- IdModulo: 1
  - NombreModulo: CRUD Estudiantes

3. Operaciones: 
// Ejemplo 
- IdOp: 1
   - NombreOp: Ver
   - IdModulo: 1 // Pertenece al Modulo CRUD Estudiantes
 

4. RolOperacion:
// Ejemplo
- dRolOp: 1
  - IdRol: 1 // le pertenece al rol Docente
  - IdOp: 1 

Una vez haberte dado la estructura de mis tablas y simulando un Create de un Rol y que apartir de ese rol se complenten las demás tablas, 
te brindo a continuación como quiero la estructura del formulario Create.cshtml:
 
                <div class="col-6">
                    <label class="col-form-label">Nombre del Rol</label>
               
                    <input asp-for="Rol.NombreRol" type="text" class="form-control" placeholder="Nombre del Rol" required>
                </div>

            </div>

        </div>

         <h4 class="fw-bold fs-4">Accesos</h4>
  
   
                <h4 class="fw-bold fs-5">Modulo CRUD Estudiantes</h4>

         
            <div class="form-check">
                <input class="form-check-input" type="checkbox" value="" id="flexCheckDefault"> 
                <label class="form-check-label" for="flexCheckDefault">
                    Ver
                </label>
            </div>

            <div class="form-check">
                <input class="form-check-input" type="checkbox" value="" id="flexCheckDefault">
                <label class="form-check-label" for="flexCheckDefault">
                    Crear
                </label>
            </div>

            <div class="form-check">
                <input class="form-check-input" type="checkbox" value="" id="flexCheckDefault">
                <label class="form-check-label" for="flexCheckDefault">
                    Editar
                </label>
            </div>

            <div class="form-check">
                <input class="form-check-input" type="checkbox" value="" id="flexCheckDefault">
                <label class="form-check-label" for="flexCheckDefault">
                    Reportar
                </label>
            </div>

            <div class="form-check">
                <input class="form-check-input" type="checkbox" value="" id="flexCheckDefault">
                <label class="form-check-label" for="flexCheckDefault">
                    Eliminar
                </label>
            </div>
        </div>
    </div>

Ayudame ha estructurar el POST Create basado en la estructura que te he brindado

*/
