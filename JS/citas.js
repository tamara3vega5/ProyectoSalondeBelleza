// =======================================================
// citas.js (Versión Final totalmente adaptada a tu API .NET)
// =======================================================

const API_SERVICIOS = "https://localhost:7024/api/Servicios";
const API_ESTILISTAS = "https://localhost:7024/api/Estilistas";
const API_CITAS = "https://localhost:7024/api/Citas";

// =======================================================
// 1. Cargar servicios al iniciar
// =======================================================
document.addEventListener("DOMContentLoaded", async () => {
    await cargarServicios();
});

async function cargarServicios() {
    const servicioSelect = document.getElementById("servicio");

    try {
        const res = await fetch(API_SERVICIOS);
        if (!res.ok) throw new Error("Error obteniendo servicios");
        const servicios = await res.json();

        servicioSelect.innerHTML = `<option value="">Seleccione un servicio</option>`;

        servicios.forEach(s => {
            servicioSelect.innerHTML += `
                <option value="${s.idServicio}" data-nombre="${s.nombreServicio}">
                    ${s.nombreServicio} - $${s.precio}
                </option>
            `;
        });

    } catch (error) {
        console.error("Error cargando servicios:", error);
    }
}

document.getElementById("servicio").addEventListener("change", cargarEstilistasFiltrados);

// =======================================================
// 2. Filtrar estilistas según el servicio elegido
// =======================================================
async function cargarEstilistasFiltrados() {
    const servicioSelect = document.getElementById("servicio");
    const estilistaSelect = document.getElementById("estilista");

    const idServicio = servicioSelect.value;

    if (!idServicio) {
        estilistaSelect.innerHTML = `<option value="">Seleccione un estilista</option>`;
        return;
    }

    const nombreServicio = servicioSelect.options[servicioSelect.selectedIndex].dataset.nombre.toLowerCase();
    let especialidadBuscada = "";

    // Mapeo automático
    if (nombreServicio.includes("acr")) especialidadBuscada = "Uñas";
    else if (nombreServicio.includes("uña")) especialidadBuscada = "Uñas";
    else if (nombreServicio.includes("corte")) especialidadBuscada = "Cabello";
    else if (nombreServicio.includes("tinte")) especialidadBuscada = "Cabello";
    else if (nombreServicio.includes("kerat")) especialidadBuscada = "Cabello";
    else if (nombreServicio.includes("facial")) especialidadBuscada = "Faciales";
    else if (nombreServicio.includes("maquillaje")) especialidadBuscada = "Maquillaje";

    try {
        const res = await fetch(API_ESTILISTAS);
        const estilistas = await res.json();

        const filtrados = estilistas.filter(e =>
            especialidadBuscada === "" ||
            (e.especialidad && e.especialidad.toLowerCase() === especialidadBuscada.toLowerCase())
        );

        estilistaSelect.innerHTML = `<option value="">Seleccione un estilista</option>`;

        if (filtrados.length === 0) {
            estilistaSelect.innerHTML += `<option value="">No hay estilistas disponibles</option>`;
            return;
        }

        filtrados.forEach(e => {
            estilistaSelect.innerHTML += `
                <option value="${e.idEstilista}">
                    ${e.nombre} (${e.especialidad})
                </option>
            `;
        });

    } catch (error) {
        console.error("Error cargando estilistas:", error);
    }
}

// =======================================================
// 3. Enviar cita al backend
// =======================================================
document.getElementById("citaForm").addEventListener("submit", async (e) => {
    e.preventDefault();

    const usuario = JSON.parse(localStorage.getItem("usuario"));
    if (!usuario) return alert("Debe iniciar sesión primero.");

    const idCliente = usuario.idCliente;
    const idServicio = document.getElementById("servicio").value;
    const idEstilista = document.getElementById("estilista").value;
    const fechaInput = document.getElementById("fecha").value;

    if (!idServicio || !idEstilista || !fechaInput) {
        alert("Complete todos los campos.");
        return;
    }

    const fechaDate = new Date(fechaInput);

    // Validación de fecha
    if (fechaDate < new Date()) {
        alert("No puede seleccionar una fecha pasada.");
        return;
    }

    const cita = {
        idCliente: idCliente,
        idServicio: parseInt(idServicio),
        idEstilista: parseInt(idEstilista),
        fecha: fechaDate.toISOString(), // formato aceptado por tu API
        estado: "Confirmado"
    };

    try {
        const res = await fetch(API_CITAS, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(cita)
        });

        const data = await res.json();

        if (!res.ok) {
            alert(data.mensaje || "No se pudo registrar la cita.");
            return;
        }

        alert("Cita registrada exitosamente.");
        window.location.href = "index.html";

    } catch (error) {
        console.error("Error enviando cita:", error);
        alert("Error conectando al servidor.");
    }
});
