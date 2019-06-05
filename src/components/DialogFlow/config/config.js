export default {
    app: {
        gateway: "https://stefanstestagent-nubiii.gateway.dialogflow.cloud.ushakov.co", // <- enter your gateway URL here, the function is just a helper function for my cloud integration. You don't normally need it
        muted: false, // <- mute microphone at start
        start_suggestions: [], // <- array of suggestions, displayed at the start screen
        fallback_lang: 'nl' // <- fallback language code, if history mode or network is unavailable
    },
    i18n: {
        en: {
            welcomeTitle: "Welcome to",
            muteTitle: "Mute Toggle",
            inputTitle: "Type your message",
            sendTitle: "Send",
            microphoneTitle: "Voice Input"
        },
        nl: {
            welcomeTitle: "Welkom bij",
            muteTitle: "Mute switch",
            inputTitle: "Typ je bericht in",
            sendTitle: "Verstuur",
            microphoneTitle: "Stem input"            
        }
    }
}