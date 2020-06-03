<template>
    <ColabDataTable title="Portions"
                    editor-title-suffix="portion"
                    store-name="portions"
                    :headers="mainHeaders"
                    :before-edit="beforeEdit"
                    :could-add="false"
                    :could-remove="false">
        <template v-slot:filter="{ filter, loading }">
            <!--
            <v-radio-group
              v-model="filter.state"
              :disabled="loading"
              row
              hide-details
            >
              <v-radio label="Have to specify" value="0" />
              <v-radio label="Not specified" value="1" />
              <v-radio label="Specified" value="2" />
            </v-radio-group>
                  -->
        </template>
        <template v-slot:editor="{ item }">
            <v-row>
                <VeeTextField v-model="item.foodId"
                              :label="mainHeaders.filter(h => h.value == 'foodId')[0].text"
                              readonly />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.foodName"
                              :label="mainHeaders.filter(h => h.value == 'foodName')[0].text"
                              readonly />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.servingId"
                              :label="mainHeaders.filter(h => h.value == 'servingId')[0].text"
                              readonly />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.servingDescription"
                              :label="mainHeaders.filter(h => h.value == 'servingDescription')[0].text"
                              readonly />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.vegetables"
                              label="Vegetables"
                              rules="decimal"
                              number />
            </v-row>
            <v-row>
                <v-col>
                    <span>FoodDoc value</span>
                </v-col>
                <v-col>
                    <span>FatSecret value</span>
                </v-col>
            </v-row>
            <v-row>
                <v-col>
                    <VeeTextField v-model="item.calories"
                                  label="Calories"
                                  rules="decimal"
                                  number />
                </v-col>
                <v-col>
                    <v-text-field v-if="item.fatSecretServing"
                                  v-model="item.fatSecretServing.calories"
                                  :disabled="true"/>

                </v-col>
            </v-row>
            <v-row>
                <v-col>
                    <VeeTextField v-model="item.carbohydrate"
                                  label="Carbohydrate"
                                  rules="decimal"
                                  number />
                </v-col>
                <v-col>
                    <v-text-field v-if="item.fatSecretServing"
                                  v-model="item.fatSecretServing.carbohydrate"
                                  :disabled="true" />
                </v-col>
            </v-row>
            <v-row>
                <v-col>
                    <VeeTextField v-model="item.protein"
                                  label="Protein"
                                  rules="decimal"
                                  number />
                </v-col>
                <v-col>
                    <v-text-field v-if="item.fatSecretServing"
                                  v-model="item.fatSecretServing.protein"
                                  :disabled="true" />
                </v-col>
            </v-row>
            <v-row>
                <v-col>
                    <VeeTextField v-model="item.fat"
                                  label="Fat"
                                  rules="decimal"
                                  number />

                </v-col>
                <v-col>
                    <v-text-field v-if="item.fatSecretServing"
                                  v-model="item.fatSecretServing.fat"
                                  :disabled="true" />
                </v-col>
            </v-row>
            <v-row>
                <v-col>
                    <VeeTextField v-model="item.saturatedFat"
                                  label="Saturated fat"
                                  rules="decimal"
                                  number />
                </v-col>
                <v-col>
                    <v-text-field v-if="item.fatSecretServing"
                                  v-model="item.fatSecretServing.saturatedFat"
                                  :disabled="true" />
                </v-col>
            </v-row>
            <v-row>
                <v-col>
                    <VeeTextField v-model="item.polyunsaturatedFat"
                                  label="Polyunsaturated fat"
                                  rules="decimal"
                                  number />
                </v-col>
                <v-col>
                    <v-text-field v-if="item.fatSecretServing"
                                  v-model="item.fatSecretServing.polyunsaturatedFat"
                                  :disabled="true" />
                </v-col>
            </v-row>
            <v-row>
                <v-col>
                    <VeeTextField v-model="item.monounsaturatedFat"
                                  label="Monounsaturated fat"
                                  rules="decimal"
                                  number />
                </v-col>
                <v-col>
                    <v-text-field v-if="item.fatSecretServing"
                                  v-model="item.fatSecretServing.monounsaturatedFat"
                                  :disabled="true" />
                </v-col>
            </v-row>
            <v-row>
                <v-col>
                    <VeeTextField v-model="item.cholesterol"
                                  label="Cholesterol"
                                  rules="decimal"
                                  number />
                </v-col>
                <v-col>
                    <v-text-field v-if="item.fatSecretServing"
                                  v-model="item.fatSecretServing.cholesterol"
                                  :disabled="true" />
                </v-col>
            </v-row>
            <v-row>
                <v-col>
                    <VeeTextField v-model="item.sodium"
                                  label="Sodium"
                                  rules="decimal"
                                  number />
                </v-col>
                <v-col>
                    <v-text-field v-if="item.fatSecretServing"
                                  v-model="item.fatSecretServing.sodium"
                                  :disabled="true" />
                </v-col>
            </v-row>
            <v-row>
                <v-col>
                    <VeeTextField v-model="item.potassium"
                                  label="Potassium"
                                  rules="decimal"
                                  number />
                </v-col>
                <v-col>
                    <v-text-field v-if="item.fatSecretServing"
                                  v-model="item.fatSecretServing.potassium"
                                  :disabled="true" />
                </v-col>
            </v-row>
            <v-row>
                <v-col>
                    <VeeTextField v-model="item.fiber"
                                  label="Fiber"
                                  rules="decimal"
                                  number />
                </v-col>
                <v-col>
                    <v-text-field v-if="item.fatSecretServing"
                                  v-model="item.fatSecretServing.fiber"
                                  :disabled="true" />
                </v-col>
            </v-row>
            <v-row>
                <v-col>
                    <VeeTextField v-model="item.sugar"
                                  label="Sugar"
                                  rules="decimal"
                                  number />
                </v-col>
                <v-col>
                    <v-text-field v-if="item.fatSecretServing"
                                  v-model="item.fatSecretServing.sugar"
                                  :disabled="true" />
                </v-col>
            </v-row>
        </template>
    </ColabDataTable>
</template>

<script>
    import Vue from 'vue'
    import integration from "@/integration";

    export default {
        components: {
            ColabDataTable: () => import("@/components/signalR/ColabRDataTable"),
            VeeTextField: () => import("@/components/inputs/VeeTextField")
        },

        data: () => ({
            mainHeaders: [
                {
                    sortable: true,
                    value: "id",
                    text: "Id"
                },
                {
                    sortable: true,
                    value: "foodId",
                    text: "Food Id"
                },
                {
                    sortable: true,
                    value: "foodName",
                    text: "Food name"
                },
                {
                    sortable: true,
                    value: "servingId",
                    text: "Serving Id"
                },
                {
                    sortable: true,
                    text: "Serving description",
                    value: "servingDescription"
                },
                {
                    sortable: true,
                    text: "Last synchronized",
                    value: "lastSynchronized",
                    align: "right"
                }
            ]
        }),
        methods: {
            async beforeEdit(item) {
                let response = await integration.servings.get({ foodId: item.foodId, servingId: item.servingId });
                if (response.status !== 200) {
                    throw new Error(`undefined error in backend (${response.status})`);
                }

                item.fatSecretServing = response.data;
            }
        }
    }
</script>
