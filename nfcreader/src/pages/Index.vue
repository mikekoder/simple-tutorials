<template>
  <q-page class="q-ma-lg q-pa-lg">
    <div class="row">
      Raw:
      <pre>{{ raw }}</pre>
    </div>
    <div class="row">
      Type: {{ type }}
    </div>
    <div class="row">
      Payload: {{ payload }}
    </div>
  </q-page>
</template>

<style>
</style>

<script>
export default {
  name: 'PageIndex',
  data(){
    return {
      type: '',
      payload: '',
      raw: ''
    }
  },
  created(){
    nfc.addNdefListener (
        this.onNfc,
        () => {},
        this.onFailure
    );
  },
  methods:{
    onNfc(nfcEvent){
      this.raw = JSON.stringify(nfcEvent.tag, null, 2);
      this.type = this.byteArrayToString(nfcEvent.tag.ndefMessage[0].type);
      this.payload = this.byteArrayToString(nfcEvent.tag.ndefMessage[0].payload);
    },
    onFailure(){

    },
    byteArrayToString(bytes){
      var str = '';
      bytes.forEach(b => {
        str += String.fromCharCode(b);
      });

      return str;
    }
  }
}
</script>
