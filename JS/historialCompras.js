/* ============================================================
      historialCompras.js – Panel Admin de Compras MatchaSalon
      Conexión 100% real a la API .NET
============================================================ */

const API_VENTAS = "https://localhost:7024/api/Ventas";
const API_DETALLE = "https://localhost:7024/api/DetalleVenta";

/* ============================================================
      1) Cargar compras al iniciar sección
============================================================ */
async function cargarCompras() {
    const tbody = document.getElementById("compras-body");
    if (!tbody) return;

    try {
        const res = await fetch(API_VENTAS);
        const ventas = await res.json();

        tbody.innerHTML = "";

        ventas.forEach(v => {
            const fecha = new Date(v.fecha).toLocaleString();

            tbody.innerHTML += `
                <tr>
                    <td>${v.idVenta}</td>
                    <td>${v.cliente?.nombre || "N/A"}</td>
                    <td>${fecha}</td>
                    <td>$${v.total}</td>
                    <td><button class="btn-primary" onclick="verDetalleCompra(${v.idVenta})">🔍 Ver</button></td>
                </tr>
            `;
        });

    } catch (error) {
        console.error("Error cargando compras:", error);
    }
}

/* ============================================================
      2) Ver detalle de una compra
============================================================ */
async function verDetalleCompra(idVenta) {
    const contenedor = document.getElementById("detalle-compra");
    const tbody = document.getElementById("detalle-body");

    if (!contenedor || !tbody) return;

    try {
        const res = await fetch(`${API_DETALLE}/venta/${idVenta}`);
        const detalle = await res.json();

        tbody.innerHTML = "";

        detalle.forEach(d => {
            tbody.innerHTML += `
                <tr>
                    <td>${d.producto?.nombreProducto || "N/A"}</td>
                    <td>$${d.producto?.precio || 0}</td>
                    <td>${d.cantidad}</td>
                    <td>$${d.subtotal}</td>
                </tr>
            `;
        });

        contenedor.style.display = "block";

    } catch (error) {
        console.error("Error cargando detalle:", error);
    }
}

/* ============================================================
      3) Cerrar detalle
============================================================ */
document.getElementById("cerrar-detalle")?.addEventListener("click", () => {
    document.getElementById("detalle-compra").style.display = "none";
});

/* ============================================================
      4) Inicializar cuando se abra sección Compras
============================================================ */
document.addEventListener("DOMContentLoaded", () => {

    // Detectar si admin navegó a la sección "Compras"
    const comprasBtn = document.querySelector('li[data-section="compras"]');

    if (comprasBtn) {
        comprasBtn.addEventListener("click", () => {
            cargarCompras();
        });
    }
});
