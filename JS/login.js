/* ============================================================
   LOGIN.JS – AUTENTICACIÓN MATCHASALON
   Conectado a API/Auth/login (DTO: LoginDto & LoginResponseDto)
============================================================ */

const API_LOGIN = "https://localhost:7024/api/Auth/login";

// ------------------------------------------------------------
// EVENTO SUBMIT DEL FORMULARIO
// ------------------------------------------------------------
document.getElementById("loginForm").addEventListener("submit", async (e) => {
    e.preventDefault();

    const correo = document.getElementById("correo").value.trim().toLowerCase();
    const password = document.getElementById("password").value.trim();

    if (!correo || !password) {
        alert("Por favor completa todos los campos.");
        return;
    }

    try {
        // ------------------------------------------------------------
        // PETICIÓN AL BACKEND
        // ------------------------------------------------------------
        const res = await fetch(API_LOGIN, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ correo, password })
        });

        const data = await res.json();

        if (!res.ok) {
            alert(data.mensaje || "Credenciales incorrectas");
            return;
        }

        const cliente = data.cliente;

        // ------------------------------------------------------------
        // GUARDAR SESIÓN EN LOCALSTORAGE
        // ------------------------------------------------------------
        const usuarioSesion = {
            idCliente: cliente.idCliente,
            nombre: cliente.nombre,
            correo: cliente.correo,
            telefono: cliente.telefono,
            rol: cliente.rol || "cliente" // asegura que tenga rol
        };

        localStorage.setItem("usuario", JSON.stringify(usuarioSesion));

        // ------------------------------------------------------------
        // REDIRECCIONAR POR ROL
        // ------------------------------------------------------------
        if (usuarioSesion.rol === "admin") {
            window.location.href = "admin.html";
        } else {
            window.location.href = "index.html";
        }

    } catch (error) {
        console.error("Error en login:", error);
        alert("Error conectando con el servidor. Intenta más tarde.");
    }
});
