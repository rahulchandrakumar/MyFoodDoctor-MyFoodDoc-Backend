<template>
    <div>
        <span v-if="title != null">{{ title }}</span>
        <div v-if="icon != null" style="text-align: center">
            <v-icon>{{ icon }}</v-icon>
        </div>
        <ul>
            <li v-for="availableItem in availableItems">
                <input type="checkbox" v-bind:checked="isListItemChecked(availableItem.id)" v-on:change="toggleListItemCheckBox($event)" v-bind:id="availableItem.id" />
                <label>{{ getLabel(availableItem) }}</label>
            </li>
        </ul>
    </div>
</template>

<script>
    export default {
        props: {
            title: {
                type: String
            },
            icon: {
                type: String
            },
            labelField: {
                type: String
            },
            availableItems: {
                type: Array
            },
            checkedItems: {
                type: Array
            },
        },
        methods: {
            isListItemChecked(id) {
                if (this.checkedItems)
                    for (var i = 0; i < this.checkedItems.length; i++) {
                        if (this.checkedItems[i] == id) {
                            return true;
                        };
                    }

                return false;
            },
            toggleListItemCheckBox(event) {
                var id = Number.parseInt(event.target.id);

                if (event.target.checked) {
                    this.checkedItems.push(id);
                }
                else {
                    this.removeListItem(id);
                }
            },
            removeListItem(id) {
                for (var i = this.checkedItems.length; i--;) {
                    if (this.checkedItems[i] === id) {
                        this.checkedItems.splice(i, 1);
                    }
                }
            },
            getLabel(item) {
                if (this.labelField) {
                    return item[this.labelField]
                }

                return item.name;
            }
        }
    };
</script>
