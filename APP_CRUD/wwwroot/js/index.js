////creación de un modelo
const _modeloPersonal = {
    idPersonal: 0,
    nombreCompleto: "",
    idDepartamento: 0,
    sueldo: 0,
    fechaContrato: "",
}

//Función para mostrar la lista de personal
function MostrarPersonal() {
    fetch("/Home/listaPersonal") //ejecutar nuestra solicitud (listaPersonal)
        .then(response => { //esperando una respuesta, ya que son promesa dentro de fetch
            return response.ok ? response.json() : Promise.reject(response) 
        })                 //si la respueta es exitosa entonces que devuelva la respuesta
                           //en formato JSon, Caso contrario que cancele esa promesa
        .then(responseJson => {
            if (responseJson.length > 0) {  //si hay registros
                $("#tablaPersonal tbody").html(""); //limpiando la labla
                responseJson.forEach((perso) => {
                    $("#tablaPersonal tbody").append(
                        $("<tr>").append(
                            $("<td>").text(perso.nombrePerso),
                            $("<td>").text(perso.refDepartamento.nombreDepa),
                            $("<td>").text(perso.sueldo),
                            $("<td>").text(perso.fechaContrato),
                            $("<td>").append(
                                $("<button>").addClass("btn btn-primary btn-sm boton-editar-personal")
                                    .text("Editar").data("dataPersonal", perso),
                                $("<button>").addClass("btn btn-danger btn-sm ms-2 boton-eliminar-personal")
                                    .text("Eliminar").data("dataPersonal", perso),
                            )
                        )
                    )
                })
            }
        })
}
//entrando al evento cuando toda la página ya ha sido cargada ejecute algunas acciones
document.addEventListener("DOMContentLoaded", function () {
    MostrarPersonal();

    fetch("/Home/listaDepartamentos")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {
            if (responseJson.length > 0) {
                responseJson.forEach((item) => {
                    $("#cboDepartamento").append(
                        $("<option>").val(item.idDepartamento).text(item.nombreDepa)
                    )
                })
            }
        })

    $("#txtFechaContrato").datepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        todayHighlight: true
    })
}, false)


function MostrarModal() {
    $("#txtNombreCompleto").val(_modeloPersonal.nombreCompleto);
    $("#cboDepartamento").val(_modeloPersonal.idDepartamento == 0 ? $("#cboDepartamento option:first").val() : _modeloPersonal.idDepartamento);
    $("#txtSueldo").val(_modeloPersonal.sueldo);
    $("#txtFechaContrato").val(_modeloPersonal.fechaContrato)

    $("#modalPersonal").modal("show");
}

$(document).on("click", ".boton-nuevo-personal", function () {
    _modeloPersonal.idPersonal = 0;
    _modeloPersonal.nombreCompleto = "";
    _modeloPersonal.idDepartamento = 0;
    _modeloPersonal.sueldo = 0;
    _modeloPersonal.fechaContrato = "";

    MostrarModal();
})

$(document).on("click", ".boton-editar-personal", function () {

    const _persona = $(this).data("dataPersonal");

    _modeloPersonal.idPersonal = _persona.idPersonal;
    _modeloPersonal.nombreCompleto = _persona.nombrePerso;
    _modeloPersonal.idDepartamento = _persona.refDepartamento.idDepartamento;
    _modeloPersonal.sueldo = _persona.sueldo;
    _modeloPersonal.fechaContrato = _persona.fechaContrato;

    MostrarModal();
})

$(document).on("click", ".boton-guardar-cambios-personal", function () {
    const modelo = {
        idPersonal: _modeloPersonal.idPersonal,
        nombrePerso: $("#txtNombreCompleto").val(),
        refDepartamento: {
            idDepartamento: $("#cboDepartamento").val()
        },
        sueldo: $("#txtSueldo").val(),
        fechaContrato: $("#txtFechaContrato").val()
    }

    if (_modeloPersonal.idPersonal == 0) {
        fetch("/Home/guardarPersonal", {
            method: "POST",
            headers: { "Content-Type": "application/json;charset=utf-8" },
            body: JSON.stringify(modelo)
        })
            .then(response => {
                return response.ok ? response.json() : Promise.reject(response)
            })
            .then(responseJson => {
                if (responseJson.valor) {
                    $("#modalPersonal").modal("hide");
                    Swal.fire("Listo!", "Personal fue creado", "success");
                    MostrarPersonal();
                }
                else
                    Swal.fire("Lo sentimos!", "No se pudo crear Personal", "error");
            })
    }
    else {
        fetch("/Home/editarPersonal", {
            method: "PUT",
            headers: { "Content-Type": "application/json;charset=utf-8" },
            body: JSON.stringify(modelo)
        })
            .then(response => {
                return response.ok ? response.json() : Promise.reject(response)
            })
            .then(responseJson => {
                if (responseJson.valor) {
                    $("#modalPersonal").modal("hide");
                    Swal.fire("Listo!", "Personal fue actualizado", "success");
                    MostrarPersonal();
                }
                else
                    Swal.fire("Lo sentimos!", "No se pudo actualizar Personal", "error");
            })
    }
})
