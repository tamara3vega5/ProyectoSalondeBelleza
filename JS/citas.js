// =======================================================
// citas.js (Versión Final Integrada con API)
// =======================================================

const API_SERVICIOS = "https://localhost:7024/api/Servicios";
const API_ESTILISTAS = "https://localhost:7024/api/Estilistas";
const API_CITAS = "https://localhost:7024/api/Citas";

// =======================================================
// 1. Cargar servicios al iniciar la página
// =======================================================
document.addEventListener("DOMContentLoaded", async () => {
    await cargarServicios();
});

async function cargarServicios() {
    const servicioSelect = document.getElementById("servicio");

    try {
        const res = await fetch(API_SERVICIOS);
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
// 2. FILTRAR ESTILISTAS SEGÚN EL SERVICIO
// =======================================================
async function cargarEstilistasFiltrados() {
    const servicioSelect = document.getElementById("servicio");
    const estilistaSelect = document.getElementById("estilista");

    const idServicio = servicioSelect.value;
    if (!idServicio) {
        estilistaSelect.innerHTML = `<option value="">Seleccione un estilista</option>`;
        return;
    }

    // Obtener el nombre del servicio
    const nombreServicio = servicioSelect.options[servicioSelect.selectedIndex].dataset.nombre.toLowerCase();

    let especialidadBuscada = "";

    // ================================================
    // MAPEO AUTOMÁTICO DE SERVICIO → ESPECIALIDAD
    // ================================================
    if (nombreServicio.includes("acrí")) especialidadBuscada = "Uñas";
    else if (nombreServicio.includes("uña")) especialidadBuscada = "Uñas";
    else if (nombreServicio.includes("corte")) especialidadBuscada = "Cabello";
    else if (nombreServicio.includes("tinte")) especialidadBuscada = "Cabello";
    else if (nombreServicio.includes("keratina")) especialidadBuscada = "Cabello";
    else if (nombreServicio.includes("facial")) especialidadBuscada = "Faciales";
    else if (nombreServicio.includes("maquillaje")) especialidadBuscada = "Maquillaje";
    else especialidadBuscada = ""; // servicio general

    try {
        const res = await fetch(API_ESTILISTAS);
        const estilistas = await res.json();

        const filtrados = estilistas.filter(e => 
            especialidadBuscada === "" 
            || (e.especialidad && e.especialidad.toLowerCase() === especialidadBuscada.toLowerCase())
        );

        estilistaSelect.innerHTML = `<option value="">Seleccione un estilista</option>`;

        if (filtrados.length === 0) {
            estilistaSelect.innerHTML = `<option value="">No hay estilistas disponibles</option>`;
            return;
        }

        filtrados.forEach(e => {
            estilistaSelect.innerHTML += `
                <option value="${e.idEstilista}">${e.nombre} (${e.especialidad})</option>
            `;
        });

    } catch (error) {
        console.error("Error cargando estilistas:", error);
    }
}

// =======================================================
// 3. ENVIAR CITA A LA API
// =======================================================
document.getElementById("citaForm").addEventListener("submit", async (e) => {
    e.preventDefault();

    const usuario = JSON.parse(localStorage.getItem("usuario"));
    if (!usuario) return alert("Debe iniciar sesión primero.");

    const idCliente = usuario.idCliente;
    const idServicio = document.getElementById("servicio").value;
    const idEstilista = document.getElementById("estilista").value;
    const fecha = document.getElementById("fecha").value;

    if (!idServicio || !idEstilista || !fecha) {
        alert("Complete todos los campos.");
        return;
    }

    const fechaDate = new Date(fecha);
    if (fechaDate < new Date()) {
        alert("No puede seleccionar una fecha pasada.");
        return;
    }

    const cita = {
        idCliente: idCliente,
        idServicio: parseInt(idServicio),
        idEstilista: parseInt(idEstilista),
        fecha: fechaDate.toISOString()
    };

    try {
        const res = await fetch(API_CITAS, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(cita)
        });

        if (!res.ok) {
            const error = await res.json();
            alert(error.mensaje || "No se pudo registrar la cita.");
            return;
        }

        alert("Cita registrada exitosamente.");
        window.location.href = "index.html";

    } catch (error) {
        console.error("Error enviando cita:", error);
        alert("Error conectando al servidor.");
    }
});
