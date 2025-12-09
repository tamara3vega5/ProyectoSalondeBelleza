/* ============================================================
              SISTEMA ADMIN COMPLETO – MATCHASALON
   CRUD Usuarios – CRUD Servicios – CRUD Productos – Reservas
   + NUEVO: CRUD ESTILISTAS
============================================================ */

/* ============================================================
      1. DATOS BASE (solo si no existen)
============================================================ */

/* ---- PRODUCTOS BASE ---- */
const productosBase = [
    { id: 1, nombre: "Shampoo Herbal", descripcion: "Repara y fortalece desde el interior.", precio: 12.00, imagen: "Imagen/shampooHerbal.jpg" },
    { id: 2, nombre: "Acondicionador Nutrisoft", descripcion: "Hidratación profunda y brillo natural.", precio: 14.00, imagen: "Imagen/acondicionadorNutrisoft.jpg" },
    { id: 3, nombre: "Cera de cabello", descripcion: "Cera para estilizar tus peinados favoritos.", precio: 15.00, imagen: "Imagen/cera.jpg" },
    { id: 4, nombre: "Gel para cabello", descripcion: "Define tus rizos todo el día.", precio: 13.00, imagen: "Imagen/gel.jpg" },
    { id: 5, nombre: "Aceite de cabello", descripcion: "Repara y reduce el frizz sin grasa.", precio: 28.00, imagen: "Imagen/aceite.jpg" },
    { id: 6, nombre: "Plancha Profesional Titanium Pro", descripcion: "Herramienta profesional para cualquier estilo.", precio: 39.99, imagen: "Imagen/plancha.jpg" },
    { id: 7, nombre: "Secadora profesional", descripcion: "Tecnología iónica doble avanzada.", precio: 39.99, imagen: "Imagen/secadora.jpg" }
];

if (!localStorage.getItem("productos")) {
    localStorage.setItem("productos", JSON.stringify(productosBase));
}

/* ---- SERVICIOS BASE ---- */
const serviciosBase = [
    { id: 1, nombre: "Manicure + Esmaltado", duracion: 40, precio: 25 },
    { id: 2, nombre: "Pedicure Spa + Esmaltado Liso", duracion: 50, precio: 20 },
    { id: 3, nombre: "Corte de Cabello", duracion: 30, precio: 30 },
    { id: 4, nombre: "Highlights", duracion: 80, precio: 80 },
    { id: 5, nombre: "Manicure + Diseños", duracion: 45, precio: 35 },
    { id: 6, nombre: "Limpieza Facial + Hidratación", duracion: 60, precio: 60 }
];

if (!localStorage.getItem("servicios")) {
    localStorage.setItem("servicios", JSON.stringify(serviciosBase));
}

/* ---- ESTILISTAS BASE (NUEVO) ---- */
const estilistasBase = [
    { id: 1, nombre: "Emilia", especialidad: "Cabello", telefono: "6000-0001", correo: "emilia@matchasalon.com" },
    { id: 2, nombre: "Carlos", especialidad: "Cabello", telefono: "6000-0002", correo: "carlos@matchasalon.com" },
    { id: 3, nombre: "Antonella", especialidad: "Uñas", telefono: "6000-0003", correo: "antonella@matchasalon.com" },
    { id: 4, nombre: "Sofía", especialidad: "Faciales", telefono: "6000-0004", correo: "sofia@matchasalon.com" },
    { id: 5, nombre: "Diego", especialidad: "Faciales", telefono: "6000-0005", correo: "diego@matchasalon.com" },
    { id: 6, nombre: "Ana", especialidad: "Maquillaje", telefono: "6000-0006", correo: "ana@matchasalon.com" }
];

if (!localStorage.getItem("estilistas")) {
    localStorage.setItem("estilistas", JSON.stringify(estilistasBase));
}

/* ============================================================
      2. UTILIDADES
============================================================ */

function guardar(key, data) { localStorage.setItem(key, JSON.stringify(data)); }
function cargar(key) { return JSON.parse(localStorage.getItem(key)) || []; }

/* ============================================================
      3. SIDEBAR – Cambiar secciones
============================================================ */

document.querySelectorAll(".sidebar-menu li").forEach(btn => {
    btn.addEventListener("click", () => {
        document.querySelectorAll(".sidebar-menu li").forEach(b => b.classList.remove("active"));
        btn.classList.add("active");

        const target = btn.dataset.section;
        document.querySelectorAll(".admin-section").forEach(sec => sec.classList.remove("active"));
        document.getElementById(target).classList.add("active");

        actualizarDashboard();
        cargarUsuarios();
        cargarServicios();
        cargarProductos();
        cargarEstilistas();
        cargarReservas();
    });
});

/* ============================================================
      4. DASHBOARD
============================================================ */

function actualizarDashboard() {
    document.getElementById("count-usuarios").textContent = cargar("usuarios").length;
    document.getElementById("count-servicios").textContent = cargar("servicios").length;
    document.getElementById("count-productos").textContent = cargar("productos").length;
    document.getElementById("count-reservas").textContent = cargar("reservas").length;
}

/* ============================================================
      5. CRUD USUARIOS
============================================================ */

function cargarUsuarios() {
    const lista = cargar("usuarios");
    const tbody = document.getElementById("usuarios-body");
    tbody.innerHTML = "";

    lista.forEach(user => {
        tbody.innerHTML += `
            <tr>
                <td>${user.nombre}</td>
                <td>${user.email}</td>
                <td>${user.rol}</td>
                <td>
                    <button onclick="editarUsuario('${user.email}')">✏️</button>
                    <button onclick="eliminarUsuario('${user.email}')">🗑️</button>
                </td>
            </tr>`;
    });
}

/* ============================================================
      6. CRUD SERVICIOS
============================================================ */

let servicios = cargar("servicios");
function saveServicios() { guardar("servicios", servicios); }

function cargarServicios() {
    const tbody = document.getElementById("servicios-body");
    tbody.innerHTML = "";

    servicios.forEach(serv => {
        tbody.innerHTML += `
            <tr>
                <td>${serv.nombre}</td>
                <td>${serv.duracion} min</td>
                <td>$${serv.precio}</td>
                <td>
                    <button onclick="cargarServicioParaEditar(${serv.id})">✏️ Editar</button>
                    <button onclick="eliminarServicio(${serv.id})">🗑️ Eliminar</button>
                </td>
            </tr>`;
    });
}

/* ---- AGREGAR SERVICIO ---- */
document.getElementById("btn-save-servicio").addEventListener("click", () => {
    const nombre = document.getElementById("serv-nombre").value.trim();
    const duracion = parseInt(document.getElementById("serv-duracion").value);
    const precio = parseFloat(document.getElementById("serv-precio").value);

    if (!nombre || !duracion || !precio) return alert("Todos los campos son obligatorios.");

    servicios.push({ id: Date.now(), nombre, duracion, precio });

    saveServicios();
    cargarServicios();

    document.getElementById("serv-nombre").value = "";
    document.getElementById("serv-duracion").value = "";
    document.getElementById("serv-precio").value = "";

    alert("Servicio agregado correctamente.");
});

/* ---- CARGAR SERVICIO EN FORMULARIO ---- */
function cargarServicioParaEditar(id) {
    const s = servicios.find(x => x.id === id);

    document.getElementById("form-edit-servicio").style.display = "block";

    document.getElementById("edit-serv-id").value = s.id;
    document.getElementById("edit-serv-nombre").value = s.nombre;
    document.getElementById("edit-serv-duracion").value = s.duracion;
    document.getElementById("edit-serv-precio").value = s.precio;
}

/* ---- EDITAR SERVICIO ---- */
document.getElementById("btn-update-servicio").addEventListener("click", () => {
    const id = parseInt(document.getElementById("edit-serv-id").value);
    const nombre = document.getElementById("edit-serv-nombre").value.trim();
    const duracion = parseInt(document.getElementById("edit-serv-duracion").value);
    const precio = parseFloat(document.getElementById("edit-serv-precio").value);

    const index = servicios.findIndex(s => s.id === id);

    servicios[index] = { id, nombre, duracion, precio };

    saveServicios();
    cargarServicios();

    document.getElementById("form-edit-servicio").style.display = "none";

    alert("Servicio actualizado correctamente.");
});

/* ---- CANCELAR ---- */
document.getElementById("btn-cancel-serv-edit").addEventListener("click", () => {
    document.getElementById("form-edit-servicio").style.display = "none";
});

/* ---- ELIMINAR SERVICIO ---- */
function eliminarServicio(id) {
    if (!confirm("¿Seguro que deseas eliminar este servicio?")) return;

    servicios = servicios.filter(s => s.id !== id);
    saveServicios();
    cargarServicios();
}

/* ============================================================
      7. CRUD PRODUCTOS
============================================================ */

let productos = cargar("productos");
function saveProductos() { guardar("productos", productos); }

function cargarProductos() {
    const tbody = document.getElementById("productos-body");
    tbody.innerHTML = "";

    productos.forEach(prod => {
        tbody.innerHTML += `
            <tr>
                <td>${prod.nombre}</td>
                <td>${prod.descripcion}</td>
                <td>$${prod.precio}</td>
                <td>
                    <button onclick="cargarProductoParaEditar(${prod.id})">✏️</button>
                    <button onclick="eliminarProducto(${prod.id})">🗑️</button>
                </td>
            </tr>`;
    });
}

/* ---- AGREGAR PRODUCTO ---- */
document.getElementById("btn-save-producto").addEventListener("click", () => {
    const nombre = document.getElementById("prod-nombre").value.trim();
    const desc = document.getElementById("prod-desc").value.trim();
    const precio = parseFloat(document.getElementById("prod-precio").value);
    const imagen = document.getElementById("prod-imagen").value.trim();

    if (!nombre || !desc || !precio || !imagen) return alert("Todos los campos son obligatorios");

    productos.push({ id: Date.now(), nombre, descripcion: desc, precio, imagen });
    saveProductos();
    cargarProductos();

    document.getElementById("prod-nombre").value = "";
    document.getElementById("prod-desc").value = "";
    document.getElementById("prod-precio").value = "";
    document.getElementById("prod-imagen").value = "";

    alert("Producto agregado correctamente.");
});

/* ---- EDITAR PRODUCTO ---- */
function cargarProductoParaEditar(id) {
    const prod = productos.find(p => p.id === id);

    document.getElementById("form-edit-producto").style.display = "block";
    document.getElementById("edit-prod-id").value = prod.id;
    document.getElementById("edit-prod-nombre").value = prod.nombre;
    document.getElementById("edit-prod-desc").value = prod.descripcion;
    document.getElementById("edit-prod-precio").value = prod.precio;
    document.getElementById("edit-prod-imagen").value = prod.imagen;
}

document.getElementById("btn-update-producto").addEventListener("click", () => {
    const id = parseInt(document.getElementById("edit-prod-id").value);
    const nombre = document.getElementById("edit-prod-nombre").value;
    const desc = document.getElementById("edit-prod-desc").value;
    const precio = parseFloat(document.getElementById("edit-prod-precio").value);
    const imagen = document.getElementById("edit-prod-imagen").value;

    const index = productos.findIndex(p => p.id === id);

    productos[index] = { id, nombre, descripcion: desc, precio, imagen };
    saveProductos();
    cargarProductos();

    document.getElementById("form-edit-producto").style.display = "none";

    alert("Producto actualizado.");
});

/* ---- CANCELAR ---- */
document.getElementById("btn-cancel-edit").addEventListener("click", () => {
    document.getElementById("form-edit-producto").style.display = "none";
});

/* ---- ELIMINAR PRODUCTO ---- */
function eliminarProducto(id) {
    if (!confirm("¿Seguro que deseas eliminar este producto?")) return;

    productos = productos.filter(p => p.id !== id);
    saveProductos();
    cargarProductos();
}

/* ============================================================
      8. CRUD ESTILISTAS (NUEVO)
============================================================ */

let estilistas = cargar("estilistas");
function saveEstilistas() { guardar("estilistas", estilistas); }

function cargarEstilistas() {
    const tbody = document.getElementById("estilistas-body");
    if (!tbody) return;

    tbody.innerHTML = "";

    estilistas.forEach(e => {
        tbody.innerHTML += `
            <tr>
                <td>${e.nombre}</td>
                <td>${e.especialidad}</td>
                <td>${e.telefono}</td>
                <td>${e.correo}</td>
                <td>
                    <button onclick="editarEstilista(${e.id})">✏️</button>
                    <button onclick="eliminarEstilista(${e.id})">🗑️</button>
                </td>
            </tr>
        `;
    });
}

document.getElementById("btn-save-estilista")?.addEventListener("click", () => {
    const nombre = document.getElementById("est-nombre").value.trim();
    const esp = document.getElementById("est-especialidad").value.trim();
    const tel = document.getElementById("est-telefono").value.trim();
    const email = document.getElementById("est-correo").value.trim();

    if (!nombre || !esp || !tel || !email)
        return alert("Todos los campos son obligatorios.");

    estilistas.push({
        id: Date.now(),
        nombre,
        especialidad: esp,
        telefono: tel,
        correo: email
    });

    saveEstilistas();
    cargarEstilistas();

    document.getElementById("est-nombre").value = "";
    document.getElementById("est-especialidad").value = "";
    document.getElementById("est-telefono").value = "";
    document.getElementById("est-correo").value = "";

    alert("Estilista agregado correctamente.");
});

function eliminarEstilista(id) {
    if (!confirm("¿Eliminar estilista?")) return;

    estilistas = estilistas.filter(e => e.id !== id);
    saveEstilistas();
    cargarEstilistas();
}

/* ============================================================
      9. CRUD RESERVAS
============================================================ */

function cargarReservas() {
    const lista = cargar("reservas");
    const tbody = document.getElementById("reservas-body");
    if (!tbody) return;

    tbody.innerHTML = "";

    lista.forEach(res => {
        tbody.innerHTML += `
            <tr>
                <td>${res.cliente}</td>
                <td>${res.servicio}</td>
                <td>${res.estilista || "N/A"}</td>
                <td>${res.fecha}</td>
                <td>${res.estado}</td>
                <td>
                    <button onclick="cambiarEstado(${res.id})">✔</button>
                    <button onclick="eliminarReserva(${res.id})">🗑️</button>
                </td>
            </tr>`;
    });
}

/* ============================================================
      10. INICIALIZACIÓN
============================================================ */

actualizarDashboard();
cargarUsuarios();
cargarServicios();
cargarProductos();
cargarEstilistas();
cargarReservas();
