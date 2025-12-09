// ===============================
//   CARGAR CARRITO DESDE STORAGE
// ===============================
let carrito = JSON.parse(localStorage.getItem("carrito")) || {};


// ===============================
//   VALIDAR CARRITO VIEJO (FIX ERROR cantidad)
// ===============================
let formatoInvalido = false;

for (const key in carrito) {
    let item = carrito[key];

    // Si el dato no es OBJETO → está en formato viejo (por ej: "Servicio": 2)
    if (typeof item !== "object" || item === null) {
        formatoInvalido = true;
        break;
    }

    // Si falta precio o cantidad → también es inválido
    if (!item.hasOwnProperty("precio") || !item.hasOwnProperty("cantidad")) {
        formatoInvalido = true;
        break;
    }
}

if (formatoInvalido) {
    console.warn("🧹 Carrito viejo detectado → limpiando...");
    carrito = {};
    localStorage.setItem("carrito", JSON.stringify(carrito));
}


// ===============================
//   ACTUALIZAR CONTADOR NAVBAR
// ===============================
function actualizarContador() {
    let total = 0;

    for (const key in carrito) {
        if (carrito[key] && typeof carrito[key].cantidad === "number") {
            total += carrito[key].cantidad;
        }
    }

    const badge = document.getElementById("cart-count");
    if (badge) badge.textContent = total;
}

actualizarContador();


// ===============================
//   GUARDAR CARRITO
// ===============================
function guardarCarrito() {
    localStorage.setItem("carrito", JSON.stringify(carrito));
}


// ===============================
//   SINCRONIZAR CANTIDADES EN LAS CARDS
// ===============================
function sincronizarServicios() {
    const displays = document.querySelectorAll(".qty-display");

    displays.forEach(display => {
        let nombre = display.dataset.nombre;

        display.textContent = carrito[nombre]
            ? carrito[nombre].cantidad
            : "0";
    });
}
document.addEventListener("DOMContentLoaded", sincronizarServicios);


// ===============================
//   AGREGAR PRODUCTO
// ===============================
function agregarProducto(nombre, precio) {
    if (!carrito[nombre]) {
        carrito[nombre] = { cantidad: 1, precio: precio };
    } else {
        carrito[nombre].cantidad++;
    }

    guardarCarrito();
    actualizarContador();
    sincronizarServicios();
    cargarCarritoEnPagina();
}


// ===============================
//   QUITAR PRODUCTO
// ===============================
function quitarProducto(nombre) {
    if (!carrito[nombre]) return;

    carrito[nombre].cantidad--;

    if (carrito[nombre].cantidad <= 0) {
        delete carrito[nombre];
    }

    guardarCarrito();
    actualizarContador();
    sincronizarServicios();
    cargarCarritoEnPagina();
}


// ===============================
//   LISTENERS GLOBALES (+ y -)
// ===============================
document.addEventListener("click", function (e) {

    if (e.target.classList.contains("plus")) {
        const nombre = e.target.dataset.nombre;
        const precio = parseFloat(e.target.dataset.precio);
        agregarProducto(nombre, precio);
    }

    if (e.target.classList.contains("minus")) {
        const nombre = e.target.dataset.nombre;
        quitarProducto(nombre);
    }

    if (e.target.id === "clear-cart") {
        carrito = {};
        guardarCarrito();
        actualizarContador();
        cargarCarritoEnPagina();
    }
});


// ===============================
//   MOSTRAR CARRITO EN carrito.html
// ===============================
function cargarCarritoEnPagina() {
    const container = document.getElementById("cart-items");
    const totalText = document.getElementById("cart-total");

    if (!container) return;

    container.innerHTML = ""; 

    let total = 0;

    for (let nombre in carrito) {
        const item = carrito[nombre];

        total += item.precio * item.cantidad;

        container.innerHTML += `
            <div class="cart-item">
                <span class="cart-item-name">${nombre}</span>

                <div class="cart-item-controls">
                    <button class="minus" data-nombre="${nombre}">−</button>
                    <span>${item.cantidad}</span>
                    <button class="plus" data-nombre="${nombre}" data-precio="${item.precio}">+</button>
                </div>

                <span class="cart-item-price">$${(item.precio * item.cantidad).toFixed(2)}</span>
            </div>
        `;
    }

    totalText.textContent = `$${total.toFixed(2)}`;
}

document.addEventListener("DOMContentLoaded", cargarCarritoEnPagina);
