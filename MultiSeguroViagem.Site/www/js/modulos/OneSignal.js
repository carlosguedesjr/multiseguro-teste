function configuraOneSignal() {
    var oneSignal = window.OneSignal || [];
    oneSignal.push(function() {
        oneSignal.init({
            appId: "6b4229af-a68a-4447-8f7c-81d68c6401d4",
            safari_web_id: "web.onesignal.auto.4f832ce8-c167-4c63-9514-5546a8912edb",
            autoRegister: true,
            notifyButton: {
                enable: false /* Set to false to hide */
            },
            persistNotification: true,
            welcomeNotification: {
                disable: true
            },
            promptOptions: {
                /* actionMessage limited to 90 characters */
                actionMessage: "PESQUISA: Você gosta Cupom de Desconto?",
                /* acceptButtonText limited to 15 characters */
                acceptButtonText: "SIM",
                /* cancelButtonText limited to 15 characters */
                cancelButtonText: "NÃO"
            }
        });
    });
};

function enviaTagOneSignal(oneSignal, obj) {
    if(oneSignal != undefined) {
        oneSignal.sendTags(obj);  
    }
};

function deletaTagsOneSignal(oneSignal, obj) {
    if(oneSignal != undefined) {
        oneSignal.deleteTags(obj);  
    }
};