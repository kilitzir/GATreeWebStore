console.log("Testdata create js");
function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}
Pparameters = "";
async function appVue1() {
    console.log('appVue1');
    //await sleep(2000);
    
    var app = new Vue({

        el: '#aaa',
        data: {
            parameters: [
                {
                    name: 'attribute name',
                    value: 'attribute parameter'
                }
            ]
        },
        methods: {
            CopyParameters() {
                console.log('appVue: CopyParameters');
                $("#TestData_params").val(JSON.stringify(this.parameters));
            },
            AddNewParameter() {
                this.parameters.push({
                    name: 'attribute name ',
                    value: 'attribute parameter '
                }
                );
            },
            DeleteParameter(index) {
                console.log("appVue:DeleteParameter");
                this.parameters.splice(index, 1);
            }
        }
    });
}

appVue1();

