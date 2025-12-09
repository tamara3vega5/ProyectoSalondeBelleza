/* ===========================================================
   JS/session.js  (Versión Final CORREGIDA 2025)
   -----------------------------------------------------------
   - Muestra usuario logueado (admin o cliente)
   - Render dinámico del botón "Salir" + "Panel"
   - Compatible con login.js actualizado
   - Evita doble ejecución del DOMContentLoaded
   =========================================================== */


/* ===========================================================
   FUNCIÓN PRINCIPAL PARA MOSTRAR EL USUARIO EN LA WEB
   =========================================================== */
function renderUserArea() {

    const userArea = document.getElementById("user-area");
    if (!userArea) return;  // La página no tiene cuadro de usuario

    const usuario = JSON.parse(localStorage.getItem("usuario"));
    if (!usuario) {
        userArea.innerHTML = `
            <a href="login.html" 
               style="color:#5E925C; font-weight:700;">
               Iniciar sesión
            </a>`;
        return;
    }

    /* =======================================================
       CASO: ADMINISTRADOR
       ======================================================= */
    if (usuario.rol === "admin") {

        userArea.innerHTML = `
            <span style="color:#5E925C; font-weight:700;">Admin</span>

            <a href="admin.html"
               style="margin-left:10px; background:#8dce88;
                      padding:6px 10px; border-radius:8px;
                      color:white; font-weight:700;">
                Panel
            </a>

            <button id="logout-btn"
                style="margin-left:10px; background:#ff5b5b; border:none;
                       color:white; padding:6px 10px; border-radius:8px; cursor:pointer;">
                Salir
            </button>
        `;
    }

    /* =======================================================
       CASO: CLIENTE NORMAL
       ======================================================= */
    else {
        userArea.innerHTML = `
            <span style="color:#5E925C; font-weight:700;">
                Hola, ${usuario.nombre}
            </span>

            <button id="logout-btn"
                style="margin-left:10px; background:#ff5b5b; border:none;
                       color:white; padding:6px 10px; border-radius:8px; cursor:pointer;">
                Salir
            </button>
        `;
    }

    // Evento salir
    const logoutBtn = document.getElementById("logout-btn");
    if (logoutBtn) {
        logoutBtn.addEventListener("click", () => {
            localStorage.removeItem("usuario");
            window.location.href = "login.html";
        });
    }
}


/* ===========================================================
   MANEJO ESPECIAL PARA admin.html (protección)
   =========================================================== */
function validarAdmin() {

    // Verifica si es admin y si está en admin.html
    if (!window.location.pathname.includes("admin.html")) return;

    const usuario = JSON.parse(localStorage.getItem("usuario"));

    if (!usuario || usuario.rol !== "admin") {
        alert("Acceso denegado — Solo administradores.");
        window.location.href = "index.html";
        return;
    }
}


/* ===========================================================
   EJECUTAR TODO AL CARGAR LA PÁGINA
   =========================================================== */
document.addEventListener("DOMContentLoaded", () => {
    validarAdmin();
    renderUserArea();
});
