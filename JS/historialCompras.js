/* ============================================================
      historialCompras.js – Panel Admin de Compras MatchaSalon
      Conexión REAL y 100% compatible con API .NET
============================================================ */

const API_VENTAS = "https://localhost:7024/api/Ventas";
const API_DETALLE = "https://localhost:7024/api/DetalleVenta";

/* ============================================================
      1) Cargar compras al abrir la sección
============================================================ */
async function cargarCompras() {
    const tbody = document.getElementById("compras-body");
    if (!tbody) return;

    try {
        const res = await fetch(API_VENTAS);
        if (!res.ok) throw new Error("Error obteniendo ventas");

        const ventas = await res.json();
        tbody.innerHTML = "";

        ventas.forEach(v => {
            tbody.innerHTML += `
                <tr>
                    <td>${v.idVenta}</td>
                    <td>${v.cliente?.nombre || "N/A"}</td>
                    <td>${new Date(v.fecha).toLocaleString()}</td>
                    <td>$${v.total.toFixed(2)}</td>
                    <td>
                        <button class="btn-primary" onclick="verDetalleCompra(${v.idVenta})">
                            🔍 Ver
                        </button>
                    </td>
                </tr>
            `;
        });

    } catch (error) {
        console.error("Error cargando compras:", error);
    }
}

/* ============================================================
      2) Ver detalle de una compra por ID
============================================================ */
async function verDetalleCompra(idVenta) {
    const contenedor = document.getElementById("detalle-compra");
    const tbody = document.getElementById("detalle-body");

    if (!contenedor || !tbody) return;

    try {
        const res = await fetch(`${API_DETALLE}/venta/${idVenta}`);
        if (!res.ok) throw new Error("Error obteniendo detalle");

        const detalle = await res.json();
        tbody.innerHTML = "";

        detalle.forEach(d => {
            tbody.innerHTML += `
                <tr>
                    <td>${d.producto?.nombreProducto || "N/A"}</td>
                    <td>$${d.producto?.precio.toFixed(2) || 0}</td>
                    <td>${d.cantidad}</td>
                    <td>$${d.subtotal.toFixed(2)}</td>
                </tr>
            `;
        });

        contenedor.style.display = "block";

    } catch (error) {
        console.error("Error cargando detalle:", error);
    }
}

/* ============================================================
      3) Cerrar ventana de detalle
============================================================ */
document.getElementById("cerrar-detalle")?.addEventListener("click", () => {
    document.getElementById("detalle-compra").style.display = "none";
});

/* ============================================================
      4) Ejecutar cuando admin abre la sección Compras
============================================================ */
document.addEventListener("DOMContentLoaded", () => {
    const botonCompras = document.querySelector('li[data-section="compras"]');

    if (botonCompras) {
        botonCompras.addEventListener("click", () => {
            cargarCompras();
        });
    }
});
