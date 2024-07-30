  'use strict';



self.addEventListener('push', function (event) {
		console.log(JSON.parse(event.data.text()));
  		const notificationObject = JSON.parse(event.data.text());//modified from tutorial to make it more dynamic
        const title = notificationObject.title;
        const options = {
            body: notificationObject.msg,
            icon: notificationObject.icon,
          image: notificationObject.image
        };
		console.log(options);
 		self.notificationURL = notificationObject.url;//modified from tutorial to make it more dynamic
  		self.id_envio = notificationObject.id_envio;

        event.waitUntil(self.registration.showNotification(title, options));
    });


self.addEventListener('notificationclick', function(event) {
  console.log('[Service Worker] Notification click Received.');

  event.notification.close();

  event.waitUntil(

		clients.openWindow("https://coteecompare.com/web-push/salvaclickusuario.php?url=" + btoa(self.notificationURL) + "&id_envio=" + self.id_envio)//modified from tutorial to make it more dynamic
	

  );
});


