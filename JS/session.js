/* ===========================================================
   JS/session.js  (Versión Final)
   -----------------------------------------------------------
   - Muestra el usuario en la barra superior.
   - Maneja el botón de salir.
   - Reconoce al administrador.
   - Funciona tanto en admin.html como en el resto del sitio.
   =========================================================== */


/* ===========================================================
   1) MANEJO ESPECIAL PARA admin.html
   =========================================================== */
if (window.location.pathname.includes("admin.html")) {

    document.addEventListener("DOMContentLoaded", () => {

        const usuario = JSON.parse(localStorage.getItem("usuarioConectado"));
        const userArea = document.getElementById("user-area");

        // Mostrar info del admin
        if (usuario && usuario.rol === "admin" && userArea) {
            userArea.innerHTML = `
                <span style="color:#5E925C; font-weight:700;">Admin</span>
                <button id="logout-btn"
                    style="margin-left:10px;background:#ff5b5b;border:none;color:white;padding:6px 10px;border-radius:8px;cursor:pointer;">
                    Salir
                </button>
            `;
        }

        // Manejo del botón de salida
        const logoutBtn = document.getElementById("logout-btn");
        if (logoutBtn) {
            logoutBtn.addEventListener("click", () => {
                localStorage.removeItem("usuarioConectado");
                window.location.href = "login.html";
            });
        }
    });

    // NO return;  admin.html también usa el bloque general de abajo
}



/* ===========================================================
   2) MOSTRAR USUARIO EN TODAS LAS OTRAS PÁGINAS
   =========================================================== */
document.addEventListener("DOMContentLoaded", () => {

    const userArea = document.getElementById("user-area");
    if (!userArea) return;

    const usuario = JSON.parse(localStorage.getItem("usuarioConectado"));
    if (!usuario) return;

    // Usuario administrador
    if (usuario.rol === "admin") {

        userArea.innerHTML = `
            <span style="color:#5E925C; font-weight:700;">Admin</span>

            <a href="admin.html" 
                style="margin-left:10px; background:#8dce88; padding:6px 10px; border-radius:8px; color:white; font-weight:700;">
                Panel
            </a>

            <button id="logout-btn"
                style="margin-left:10px; background:#ff5b5b; border:none; color:white; padding:6px 10px; border-radius:8px; cursor:pointer;">
                Salir
            </button>
        `;

    } else {

        // Usuario normal
        userArea.innerHTML = `
            <span style="color:#5E925C; font-weight:700;">Hola, ${usuario.nombre}</span>

            <button id="logout-btn"
                style="margin-left:10px; background:#ff5b5b; border:none; color:white; padding:6px 10px; border-radius:8px; cursor:pointer;">
                Salir
            </button>
        `;
    }

    // Evento salir
    const logoutBtn = document.getElementById("logout-btn");
    if (logoutBtn) {
        logoutBtn.addEventListener("click", () => {
            localStorage.removeItem("usuarioConectado");
            window.location.href = "login.html";
        });
    }
});
