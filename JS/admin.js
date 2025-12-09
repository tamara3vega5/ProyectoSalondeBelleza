/* ============================================================
   ADMIN.JS – SISTEMA ADMINISTRATIVO COMPLETO MATCHASALON
   Totalmente conectado al backend (API REST .NET)
============================================================ */

/* ===========================
      RUTAS DE API
=========================== */
const API_CLIENTES   = "https://localhost:7024/api/Clientes";
const API_SERVICIOS  = "https://localhost:7024/api/Servicios";
const API_PRODUCTOS  = "https://localhost:7024/api/Productos";
const API_ESTILISTAS = "https://localhost:7024/api/Estilistas";
const API_CITAS      = "https://localhost:7024/api/Citas";

/* ===========================
      PROTECCIÓN ADMIN
=========================== */
document.addEventListener("DOMContentLoaded", () => {

    const usuario = JSON.parse(localStorage.getItem("usuario"));
    if (!usuario || usuario.rol !== "admin") {
        alert("Acceso no autorizado.");
        window.location.href = "index.html";
        return;
    }

    inicializarSidebar();
    actualizarDashboard();
    cargarUsuarios();
    cargarEstilistas();
    cargarServicios();
    cargarProductos();
    cargarReservas();
});

/* ===========================
      SIDEBAR CAMBIO SECCIONES
=========================== */
function inicializarSidebar() {
    document.querySelectorAll(".sidebar-menu li").forEach(btn => {
        btn.addEventListener("click", () => {

            document.querySelectorAll(".sidebar-menu li").forEach(b => b.classList.remove("active"));
            btn.classList.add("active");

            const destino = btn.dataset.section;
            document.querySelectorAll(".admin-section").forEach(sec => sec.classList.remove("active"));
            document.getElementById(destino).classList.add("active");
        });
    });

    document.getElementById("logout-btn").addEventListener("click", () => {
        localStorage.removeItem("usuario");
        window.location.href = "login.html";
    });
}

/* ============================================================
   DASHBOARD (TOTAL DE REGISTROS)
============================================================ */
async function actualizarDashboard() {

    const countUsuarios  = document.getElementById("count-usuarios");
    const countServicios = document.getElementById("count-servicios");
    const countProductos = document.getElementById("count-productos");
    const countReservas  = document.getElementById("count-reservas");

    try {
        const [u, s, p, r] = await Promise.all([
            fetch(API_CLIENTES).then(r => r.json()),
            fetch(API_SERVICIOS).then(r => r.json()),
            fetch(API_PRODUCTOS).then(r => r.json()),
            fetch(API_CITAS).then(r => r.json())
        ]);

        countUsuarios.textContent  = u.length;
        countServicios.textContent = s.length;
        countProductos.textContent = p.length;
        countReservas.textContent  = r.length;

    } catch (e) {
        console.error("Error Dashboard:", e);
    }
}

/* ============================================================
   CRUD USUARIOS (CLIENTES)
============================================================ */
async function cargarUsuarios() {
    const tbody = document.getElementById("usuarios-body");
    if (!tbody) return;

    try {
        const usuarios = await fetch(API_CLIENTES).then(r => r.json());

        tbody.innerHTML = "";
        usuarios.forEach(u => {
            tbody.innerHTML += `
                <tr>
                    <td>${u.nombre}</td>
                    <td>${u.correo}</td>
                    <td>${u.rol}</td>
                    <td>
                        <button onclick="eliminarUsuario(${u.idCliente})">🗑️</button>
                    </td>
                </tr>
            `;
        });
    } catch (e) {
        console.error("Error cargando usuarios:", e);
    }
}

async function eliminarUsuario(id) {
    if (!confirm("¿Eliminar este usuario?")) return;

    try {
        await fetch(`${API_CLIENTES}/${id}`, { method: "DELETE" });
        cargarUsuarios();
        actualizarDashboard();
    } catch (e) {
        console.error("Error eliminando usuario:", e);
    }
}

/* ============================================================
   CRUD ESTILISTAS
============================================================ */
async function cargarEstilistas() {
    const tbody = document.getElementById("estilistas-body");
    if (!tbody) return;

    try {
        const data = await fetch(API_ESTILISTAS).then(r => r.json());
        tbody.innerHTML = "";

        data.forEach(e => {
            tbody.innerHTML += `
                <tr>
                    <td>${e.nombre}</td>
                    <td>${e.especialidad}</td>
                    <td>${e.telefono}</td>
                    <td>${e.correo}</td>
                    <td>
                        <button onclick='cargarEstilistaParaEditar(${e.idEstilista})'>✏️</button>
                        <button onclick='eliminarEstilista(${e.idEstilista})'>🗑️</button>
                    </td>
                </tr>
            `;
        });

    } catch (e) {
        console.error("Error cargando estilistas:", e);
    }
}

document.getElementById("btn-save-estilista").addEventListener("click", async () => {

    const nombre = document.getElementById("est-nombre").value.trim();
    const especialidad = document.getElementById("est-especialidad").value.trim();
    const telefono = document.getElementById("est-telefono").value.trim();
    const correo = document.getElementById("est-correo").value.trim();

    if (!nombre || !especialidad || !telefono || !correo)
        return alert("Todos los campos son obligatorios.");

    const estilista = { nombre, especialidad, telefono, correo };

    try {
        await fetch(API_ESTILISTAS, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(estilista)
        });

        cargarEstilistas();
        actualizarDashboard();

        document.getElementById("est-nombre").value = "";
        document.getElementById("est-especialidad").value = "";
        document.getElementById("est-telefono").value = "";
        document.getElementById("est-correo").value = "";

    } catch (e) {
        console.error("Error agregando estilista:", e);
    }
});

async function eliminarEstilista(id) {
    if (!confirm("¿Eliminar este estilista?")) return;

    try {
        await fetch(`${API_ESTILISTAS}/${id}`, { method: "DELETE" });
        cargarEstilistas();
        actualizarDashboard();
    } catch (e) {
        console.error("Error eliminando estilista:", e);
    }
}

/* ============================================================
   CRUD SERVICIOS
============================================================ */
async function cargarServicios() {
    const tbody = document.getElementById("servicios-body");
    if (!tbody) return;

    try {
        const data = await fetch(API_SERVICIOS).then(r => r.json());
        tbody.innerHTML = "";

        data.forEach(s => {
            tbody.innerHTML += `
                <tr>
                    <td>${s.nombreServicio}</td>
                    <td>${s.duracionMin} min</td>
                    <td>$${s.precio}</td>
                    <td>
                        <button onclick="cargarServicioParaEditar(${s.idServicio})">✏️ Editar</button>
                        <button onclick="eliminarServicio(${s.idServicio})">🗑️ Eliminar</button>
                    </td>
                </tr>
            `;
        });

    } catch (e) {
        console.error("Error cargando servicios:", e);
    }
}

document.getElementById("btn-save-servicio").addEventListener("click", async () => {

    const nombre = document.getElementById("serv-nombre").value.trim();
    const duracion = parseInt(document.getElementById("serv-duracion").value);
    const precio = parseFloat(document.getElementById("serv-precio").value);

    if (!nombre || !duracion || !precio) {
        return alert("Todos los campos son obligatorios.");
    }

    const nuevo = {
        nombreServicio: nombre,
        duracionMin: duracion,
        precio: precio,
        descripcion: ""
    };

    try {
        await fetch(API_SERVICIOS, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(nuevo)
        });

        cargarServicios();
        actualizarDashboard();

        document.getElementById("serv-nombre").value = "";
        document.getElementById("serv-duracion").value = "";
        document.getElementById("serv-precio").value = "";

    } catch (e) {
        console.error("Error creando servicio:", e);
    }
});

async function eliminarServicio(id) {
    if (!confirm("¿Eliminar servicio?")) return;

    try {
        await fetch(`${API_SERVICIOS}/${id}`, { method: "DELETE" });
        cargarServicios();
        actualizarDashboard();
    } catch (e) {
        console.error("Error eliminando servicio:", e);
    }
}

/* ============================================================
   CRUD PRODUCTOS
============================================================ */
async function cargarProductos() {
    const tbody = document.getElementById("productos-body");
    if (!tbody) return;

    try {
        const data = await fetch(API_PRODUCTOS).then(r => r.json());
        tbody.innerHTML = "";

        data.forEach(p => {
            tbody.innerHTML += `
                <tr>
                    <td>${p.nombreProducto}</td>
                    <td>${p.descripcion}</td>
                    <td>$${p.precio}</td>
                    <td>
                        <button onclick="cargarProductoParaEditar(${p.idProducto})">✏️</button>
                        <button onclick="eliminarProducto(${p.idProducto})">🗑️</button>
                    </td>
                </tr>
            `;
        });

    } catch (e) {
        console.error("Error cargando productos:", e);
    }
}

document.getElementById("btn-save-producto").addEventListener("click", async () => {

    const nombre = document.getElementById("prod-nombre").value.trim();
    const desc = document.getElementById("prod-desc").value.trim();
    const precio = parseFloat(document.getElementById("prod-precio").value);
    const imagen = document.getElementById("prod-imagen").value.trim();

    if (!nombre || !desc || !precio)
        return alert("Todos los campos son obligatorios.");

    const nuevo = {
        nombreProducto: nombre,
        descripcion: desc,
        precio: precio,
        stock: 20, 
        imagen: imagen
    };

    try {
        await fetch(API_PRODUCTOS, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(nuevo)
        });

        cargarProductos();
        actualizarDashboard();

        document.getElementById("prod-nombre").value = "";
        document.getElementById("prod-desc").value = "";
        document.getElementById("prod-precio").value = "";
        document.getElementById("prod-imagen").value = "";

    } catch (e) {
        console.error("Error creando product:", e);
    }
});

async function eliminarProducto(id) {
    if (!confirm("¿Eliminar producto?")) return;

    try {
        await fetch(`${API_PRODUCTOS}/${id}`, { method: "DELETE" });
        cargarProductos();
        actualizarDashboard();
    } catch (e) {
        console.error("Error eliminando producto:", e);
    }
}

/* ============================================================
   CRUD RESERVAS (CITAS)
============================================================ */
async function cargarReservas() {
    const tbody = document.getElementById("reservas-body");
    if (!tbody) return;

    try {
        const reservas = await fetch(API_CITAS).then(r => r.json());

        tbody.innerHTML = "";

        reservas.forEach(r => {

            tbody.innerHTML += `
                <tr>
                    <td>${r.cliente?.nombre || "N/A"}</td>
                    <td>${r.servicio?.nombreServicio || "N/A"}</td>
                    <td>${r.estilista?.nombre || "N/A"}</td>
                    <td>${new Date(r.fecha).toLocaleString()}</td>
                    <td>${r.estado}</td>
                    <td>
                        <button onclick="cambiarEstado(${r.idCita})">✔ Estado</button>
                        <button onclick="eliminarReserva(${r.idCita})">🗑️</button>
                    </td>
                </tr>
            `;
        });

    } catch (e) {
        console.error("Error cargando reservas:", e);
    }
}

async function eliminarReserva(id) {
    if (!confirm("¿Eliminar esta reserva?")) return;

    try {
        await fetch(`${API_CITAS}/${id}`, { method: "DELETE" });
        cargarReservas();
        actualizarDashboard();
    } catch (e) {
        console.error("Error eliminando:", e);
    }
}

async function cambiarEstado(id) {
    const nuevo = prompt("Nuevo estado: Pendiente / Completada / Cancelada", "Completada");

    if (!nuevo) return;

    const body = { idCita: id, nuevoEstado: nuevo };

    try {
        await fetch(`${API_CITAS}/estado`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(body)
        });

        cargarReservas();
    } catch (e) {
        console.error("Error cambiando estado:", e);
    }
}
