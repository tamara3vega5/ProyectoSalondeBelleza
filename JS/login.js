/* ===========================================================
   LOGIN – VERSIÓN FINAL
   -----------------------------------------------------------
   Usa la lista real en localStorage["usuarios"]
   Conserva tus usuarios y sesión normalmente
   =========================================================== */

document.getElementById("login-form")?.addEventListener("submit", function (e) {
  e.preventDefault();

  const email = document.querySelector("#login-form input[type=email]").value.trim();
  const pass  = document.querySelector("#login-form input[type=password]").value.trim();

  const usuarios = JSON.parse(localStorage.getItem("usuarios")) || [];
  const user = usuarios.find(u => u.email === email && u.password === pass);

  if (!user) {
    mostrarError("Correo o contraseña incorrectos.");
    return;
  }

  localStorage.setItem("usuarioConectado", JSON.stringify(user));

  if (user.rol === "admin") {
    window.location.href = "admin.html";
  } else {
    window.location.href = "index.html";
  }
});

function mostrarError(mensaje) {
  let errorBox = document.querySelector(".login-error");
  if (!errorBox) {
    errorBox = document.createElement("p");
    errorBox.className = "login-error";
    errorBox.style.color = "#d9534f";
    errorBox.style.fontWeight = "700";
    errorBox.style.marginTop = "10px";
    document.getElementById("login-form").appendChild(errorBox);
  }
  errorBox.textContent = mensaje;
}
