/* ============================================================
   SISTEMA DE CITAS – MATCHASALON
   Reparación automática + carga de estilistas + validación
   + FILTRAR ESTILISTAS SEGÚN SERVICIO
============================================================ */

/* ============================================================
   1. AUTO-REPARAR ESTILISTAS SI ESTÁN VACÍOS
============================================================ */
function asegurarEstilistas() {
    let estilistas = JSON.parse(localStorage.getItem("estilistas"));

    if (!estilistas || estilistas.length === 0) {
        const estilistasBase = [
            { id: 1, nombre: "Emilia", especialidad: "Cabello", telefono: "6000-0001", correo: "emilia@matchasalon.com" },
            { id: 2, nombre: "Carlos", especialidad: "Cabello", telefono: "6000-0002", correo: "carlos@matchasalon.com" },
            { id: 3, nombre: "Antonella", especialidad: "Uñas", telefono: "6000-0003", correo: "antonella@matchasalon.com" },
            { id: 4, nombre: "Sofía", especialidad: "Faciales", telefono: "6000-0004", correo: "sofia@matchasalon.com" },
            { id: 5, nombre: "Diego", especialidad: "Faciales", telefono: "6000-0005", correo: "diego@matchasalon.com" },
            { id: 6, nombre: "Ana", especialidad: "Maquillaje", telefono: "6000-0006", correo: "ana@matchasalon.com" }
        ];

        localStorage.setItem("estilistas", JSON.stringify(estilistasBase));
        estilistas = estilistasBase;
    }

    return estilistas;
}

/* ============================================================
   2. MAPEO SERVICIO → ESPECIALIDAD
============================================================ */
const servicioEspecialidad = {
    "Uñas acrílicas": "Uñas",
    "Manicure + Pedicure": "Uñas",
    "Corte de cabello": "Cabello",
    "Coloración": "Cabello",
    "Limpieza Facial + Hidratación": "Faciales",
    "Facial": "Faciales",
    "Maquillaje profesional": "Maquillaje"
};

/* ============================================================
   3. CARGAR ESTILISTAS SEGÚN EL SERVICIO SELECCIONADO
============================================================ */
function filtrarEstilistasPorServicio() {
    const estilistas = asegurarEstilistas();
    const servicioSelect = document.getElementById("servicio");
    const estilistaSelect = document.getElementById("estilista");

    servicioSelect.addEventListener("change", () => {
        const servicio = servicioSelect.value;
        const especialidadRequerida = servicioEspecialidad[servicio];

        estilistaSelect.innerHTML = `<option value="">Seleccione…</option>`;

        if (!especialidadRequerida) return;

        const filtrados = estilistas.filter(e => e.especialidad === especialidadRequerida);

        filtrados.forEach(e => {
            const opt = document.createElement("option");
            opt.value = e.nombre;
            opt.textContent = `${e.nombre} – ${e.especialidad}`;
            estilistaSelect.appendChild(opt);
        });
    });
}

/* ============================================================
   4. GUARDAR LA CITA + VALIDACIÓN DE HORARIO
============================================================ */
function iniciarSistemaCitas() {
    const form = document.getElementById("formCita");

    form.addEventListener("submit", (e) => {
        e.preventDefault();

        const nombre = document.getElementById("nombre").value.trim();
        const email = document.getElementById("email").value.trim();
        const servicio = document.getElementById("servicio").value.trim();
        const estilista = document.getElementById("estilista").value.trim();
        const fecha = document.getElementById("fecha").value;
        const hora = document.getElementById("hora").value;

        if (!nombre || !email || !servicio || !estilista || !fecha || !hora) {
            alert("Debe completar todos los campos.");
            return;
        }

        const reservas = JSON.parse(localStorage.getItem("reservas")) || [];
        const fechaHora = fecha + " " + hora;

        const ocupado = reservas.some(r =>
            r.estilista === estilista &&
            r.fecha === fechaHora
        );

        if (ocupado) {
            alert(`El estilista ${estilista} ya tiene una cita registrada a esa hora.`);
            return;
        }

        reservas.push({
            id: Date.now(),
            cliente: nombre,
            email,
            servicio,
            estilista,
            fecha: fechaHora,
            estado: "Pendiente"
        });

        localStorage.setItem("reservas", JSON.stringify(reservas));

        document.getElementById("mensajeConfirmacion").textContent =
            "¡Cita registrada exitosamente!";

        form.reset();

        setTimeout(() => {
            document.getElementById("mensajeConfirmacion").textContent = "";
        }, 2500);
    });
}

/* ============================================================
   5. INICIALIZAR TODO
============================================================ */
document.addEventListener("DOMContentLoaded", () => {
    asegurarEstilistas();
    filtrarEstilistasPorServicio();
    iniciarSistemaCitas();
});
