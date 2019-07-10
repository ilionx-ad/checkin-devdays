<template>
  <div>
    <!-- Text input  -->
    <input
      :aria-label="(config.i18n[lang()] && config.i18n[lang()].inputTitle) || config.i18n[config.app.fallback_lang].inputTitle"
      class="input"
      type="text"
      :placeholder="(config.i18n[lang()] && config.i18n[lang()].inputTitle) || config.i18n[config.app.fallback_lang].inputTitle"
      v-model="query"
      @keypress.enter="submit()"
    >
    <!-- Microphone Button -->
    <br>
    <br>
    <div
      :aria-label="(config.i18n[lang()] && config.i18n[lang()].microphoneTitle) || config.i18n[config.app.fallback_lang].microphoneTitle"
      :title="(config.i18n[lang()] && config.i18n[lang()].microphoneTitle) || config.i18n[config.app.fallback_lang].microphoneTitle"
      class="button-container mic_button"
      :class="{'mic_active': micro}"
      @click="micro = !micro"
    >
      <i class="material-icons" aria-hidden="true">Click here to enable mic</i>
    </div>
    <!-- Result -->
    <br>
    <div class="result">Show response: {{answer}}</div>
  </div>
</template>


<script>
import * as uuidv1 from "uuid/v1";

export default {
  name: "dialogflow",
  components: {},
  data() {
    return {
      app: null,
      messages: [],
      language: "",
      session: "",
      muted: this.config.app.muted,
      loading: false,
      query: "",
      answer: "",
      micro: false,
      recognition: null
    };
  },
  created() {
    /* If history is enabled, the messages are retrieved from localStorage */
    if (this.history() && localStorage.getItem("message_history") !== null) {
      this.messages = JSON.parse(localStorage.getItem("message_history"));
    }

    /* Session should be persistent (in case of page reload, the context should stay) */
    if (this.history() && localStorage.getItem("session") !== null) {
      this.session = localStorage.getItem("session");
    } else {
      this.session = uuidv1();
      if (this.history()) localStorage.setItem("session", this.session);
    }

    /* Cache Agent (this will save bandwith) */
    if (this.history() && localStorage.getItem("agent") !== null) {
      this.app = JSON.parse(localStorage.getItem("agent"));
    } else {
      fetch(this.config.app.gateway)
        .then(response => {
          return response.json();
        })
        .then(agent => {
          this.app = agent;
          if (this.history())
            localStorage.setItem("agent", JSON.stringify(agent));
        });
    }

    if (window.webkitSpeechRecognition || window.SpeechRecognition) {
      this.recognition =
        new window.webkitSpeechRecognition() || new window.SpeechRecognition();
      this.recognition.interimResults = true;
      this.recognition.lang = this.lang();
    }
  },
  computed: {
    /* The code below is used to extract suggestions from last message, to display it on ChatInput */
    suggestions() {
      if (this.messages.length > 0) {
        let last_message = this.messages[this.messages.length - 1].queryResult
          .fulfillmentMessages;
        let suggestions = [];

        for (let component in last_message) {
          if (last_message[component].name == "SUGGESTIONS")
            suggestions.text_suggestions = last_message[component].content;
          if (last_message[component].name == "LINK_OUT_SUGGESTION")
            suggestions.link_suggestion = last_message[component].content;
        }

        return suggestions;
      } else {
        return {
          text_suggestions: this.config.app.start_suggestions // <- if no messages are present, return start_suggestions, from config.js to help user figure out what he can do with your application
        };
      }
    }
  },
  watch: {
    /* This function is triggered, when new messages arrive */
    messages(messages) {
      if (this.history())
        localStorage.setItem("message_history", JSON.stringify(messages)); // <- Save history if the feature is enabled
    },
    /* This function is triggered, when request is started or finished */
    loading() {
      // <- wait for render (timeout) and then smoothly scroll #app down to #bottom selector, used as anchor
    },
    /* This function triggers when user clicks on the microphone button */
    micro(bool) {
      if (bool) {
        /* When value is true, start voice recognition */
        this.recognition.start();
        this.recognition.onresult = event => {
          for (let i = event.resultIndex; i < event.results.length; ++i) {
            this.query = event.results[i][0].transcript; // <- push results to the Text input
          }
        };

        this.recognition.onend = () => {
          this.recognition.stop();
          this.micro = false;
          this.submit(this.query); // <- submit the result
        };
      } else {
        this.recognition.abort(); // <- if user stops the recognition, abort it (in V1 this prevented users from starting a new recording)
      }
    }
  },
  methods: {
    send(q) {
      let request = {
        queryInput: {
          text: {
            text: q,
            languageCode: this.lang()
          }
        }
      }; // <- this is how a Dialogflow request look like

      this.loading = true;

      /* Make the request to gateway with formatting enabled */
      fetch(this.config.app.gateway + "/" + this.session + "?format=true", {
        method: "POST",
        headers: { "content-type": "application/json" },
        body: JSON.stringify(request)
      })
        .then(response => {
          return response.json();
        })
        .then(response => {
          this.messages.push(response);
          this.handle(response); // <- trigger the handle function (explanation below)
          this.loading = false;
          this.answer = response;
        });
    },
    handle(response) {
      /* This function is used for speech output */
      for (let component in response.queryResult.fulfillmentMessages) {
        let text; // <- init a text variable

        /* Set the text variable according to component name */
        if (
          response.queryResult.fulfillmentMessages[component].name == "DEFAULT"
        )
          text = response.queryResult.fulfillmentMessages[component].content;
        if (
          response.queryResult.fulfillmentMessages[component].name ==
          "SIMPLE_RESPONSE"
        )
          text =
            response.queryResult.fulfillmentMessages[component].content
              .textToSpeech;

        let speech = new SpeechSynthesisUtterance(text);
        speech.voiceURI = "native"; // <- change this, to get a different voice

        /* This "hack" is used to format our lang format, to some other lang format (example: en -> en_EN). Mainly for Safari, Firefox and Edge */
        speech.lang = this.lang() + "-" + this.lang().toUpperCase();

        if (!this.muted) window.speechSynthesis.speak(speech); // <- if app is not muted, speak out the speech
      }
    },
    submit() {
      console.log("query", this.query);
      if (this.query.length > 0) {
        this.send(this.query);
        this.query = "";
      }
    }
  }
};
</script>