// =======================================================
// carrito.js — Versión Final Integrada con API REAL
// =======================================================

const API_PRODUCTOS = "https://localhost:7024/api/Productos";
const API_VENTAS = "https://localhost:7024/api/Ventas";
const API_DETALLE = "https://localhost:7024/api/DetalleVenta";

// Estructura básica del carrito
let carrito = JSON.parse(localStorage.getItem("carrito")) || [];

// =======================================================
// 1. Cargar productos al iniciar
// =======================================================
document.addEventListener("DOMContentLoaded", () => {
    cargarProductos();
    mostrarCarrito();
});

async function cargarProductos() {
    const contenedor = document.getElementById("lista-productos");

    try {
        const res = await fetch(API_PRODUCTOS);
        if (!res.ok) throw new Error("Error obteniendo productos");

        const productos = await res.json();
        contenedor.innerHTML = "";

        productos.forEach(p => {
            contenedor.innerHTML += `
                <div class="producto-item">
                    <h3>${p.nombreProducto}</h3>
                    <p>${p.descripcion}</p>
                    <strong>$${p.precio}</strong><br>

                    <button onclick="agregarAlCarrito(${p.idProducto}, '${p.nombreProducto}', ${p.precio})"
                        class="btn-agregar">
                        Agregar al carrito
                    </button>
                </div>
            `;
        });

    } catch (error) {
        console.error("Error cargando productos:", error);
    }
}

// =======================================================
// 2. Agregar producto al carrito
// =======================================================
window.agregarAlCarrito = function (id, nombre, precio) {

    let item = carrito.find(p => p.id === id);

    if (item) {
        item.cantidad++;
        item.subtotal = item.cantidad * item.precio;
    } else {
        carrito.push({
            id,
            nombre,
            precio,
            cantidad: 1,
            subtotal: precio
        });
    }

    guardarCarrito();
    mostrarCarrito();
};

// =======================================================
// 3. Mostrar carrito en pantalla
// =======================================================
function mostrarCarrito() {
    const tabla = document.getElementById("carrito-body");
    const totalSpan = document.getElementById("total");

    tabla.innerHTML = "";

    let total = 0;

    carrito.forEach((item, index) => {
        if (isNaN(item.subtotal)) item.subtotal = item.precio;

        total += item.subtotal;

        tabla.innerHTML += `
            <tr>
                <td>${item.nombre}</td>
                <td>$${item.precio}</td>
                <td>${item.cantidad}</td>
                <td>$${item.subtotal}</td>
                <td>
                    <button onclick="eliminarItem(${index})" class="btn-eliminar">X</button>
                </td>
            </tr>
        `;
    });

    totalSpan.innerText = "$" + total.toFixed(2);
}

// =======================================================
// 4. Eliminar un producto del carrito
// =======================================================
window.eliminarItem = function (index) {
    carrito.splice(index, 1);
    guardarCarrito();
    mostrarCarrito();
};

// =======================================================
// 5. Guardar carrito en localStorage
// =======================================================
function guardarCarrito() {
    localStorage.setItem("carrito", JSON.stringify(carrito));
}

// =======================================================
// 6. Finalizar compra (Venta + Detalles)
// =======================================================
document.getElementById("btn-comprar").addEventListener("click", async () => {

    const usuario = JSON.parse(localStorage.getItem("usuario"));
    if (!usuario) {
        alert("Debes iniciar sesión para comprar.");
        return;
    }

    if (carrito.length === 0) {
        alert("El carrito está vacío.");
        return;
    }

    // Total de venta
    let total = carrito.reduce((sum, item) => sum + item.subtotal, 0);

    // -----------------------------
    // 1. Registrar Venta
    // -----------------------------
    const venta = {
        idCliente: usuario.idCliente,
        total: total
    };

    let ventaId = null;

    try {
        const resVenta = await fetch(API_VENTAS, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(venta)
        });

        if (!resVenta.ok) {
            alert("No se pudo registrar la venta.");
            return;
        }

        const dataVenta = await resVenta.json();
        ventaId = dataVenta.idVenta;

    } catch (error) {
        console.error("Error registrando venta:", error);
        alert("Error conectando con el servidor.");
        return;
    }

    // -----------------------------
    // 2. Registrar cada DetalleVenta
    // -----------------------------
    for (const item of carrito) {

        const detalle = {
            idVenta: ventaId,
            idProducto: item.id,
            cantidad: item.cantidad,
            subtotal: item.subtotal
        };

        try {
            const resDetalle = await fetch(API_DETALLE, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(detalle)
            });

            if (!resDetalle.ok) {
                console.error("Error detalle venta:", await resDetalle.text());
            }

        } catch (error) {
            console.error("Error registrando detalle:", error);
        }
    }

    // Vaciar carrito
    carrito = [];
    guardarCarrito();
    mostrarCarrito();

    alert("Compra realizada con éxito.");
});
