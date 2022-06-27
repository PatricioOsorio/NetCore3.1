// Toast
const showToast = (message, icon = 'success', time = 3000) => {
  const Toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    showCloseButton: true,
    timer: time,
    timerProgressBar: true,
    didOpen: (toast) => {
      toast.addEventListener('mouseenter', Swal.stopTimer);
      toast.addEventListener('mouseleave', Swal.resumeTimer);
    },
  });

  Toast.fire({
    icon: icon,
    title: message,
  });
};

document.addEventListener('click', (e) => {
  // Alerta borrar rol
  if (e.target.matches('#AlertaEliminarRol')) {
    Swal.fire({
      title: `¿Eliminar rol: <span class="text-primary">${e.target.dataset.name}</span>?`,
      text: 'No podrás revertir esto.',
      icon: 'warning',
      showCloseButton: true,
      showCancelButton: true,
      confirmButtonText: 'Sí, bórralo.',
      cancelButtonText: '¡No, cancelar!',
      confirmButtonColor: 'var(--bs-danger)',
      cancelButtonColor: 'var(--bs-secondary)',
    }).then((result) => {
      if (result.isConfirmed) {
        $.ajax({
          type: 'POST',
          url: '/Administracion/BorrarRol',
          data: { id: e.target.dataset.id },
          cache: false,
          success: function (response) {
            Swal.fire({
              title: '¡Eliminado!',
              text: 'El rol ha sido eliminado.',
              icon: 'success',
              confirmButtonColor: 'var(--bs-primary)',
            }).then(function () {
              location.href = '/Administracion/ListaRoles';
            });
          },
          error: function (error) {
            showToast("No es posible eliminar el rol, existen usuarios dentro del rol", "error", 8000)
          },
        });
      }
    });
  }

  // Alerta borrar usuario
  if (e.target.matches('#AlertaEliminarUsuario')) {
    Swal.fire({
      title: `¿Eliminar usuario: <span class="text-primary">${e.target.dataset.username}</span>?`,
      text: 'No podrás revertir esto.',
      icon: 'warning',
      showCloseButton: true,
      showCancelButton: true,
      confirmButtonText: 'Sí, bórralo.',
      cancelButtonText: '¡No, cancelar!',
      confirmButtonColor: 'var(--bs-danger)',
      cancelButtonColor: 'var(--bs-secondary)',
    }).then((result) => {
      if (result.isConfirmed) {
        $.ajax({
          type: 'POST',
          url: '/Administracion/BorrarUsuario',
          data: { id: e.target.dataset.id },
          cache: false,
          success: function (response) {
            Swal.fire({
              title: '¡Eliminado!',
              text: 'El usuario ha sido eliminado.',
              icon: 'success',
              confirmButtonColor: 'var(--bs-primary)',
            }).then(function () {
              location.href = '/Administracion/ListaUsuarios';
            });
          },
        });
      }
    });
  }
});
