<template>
    <div>
        <span>{{ title }}</span>
        <ul>
            <li v-for="availableItem in availableItems">
                <input type="checkbox" v-bind:checked="isListItemChecked(availableItem.id)" v-on:change="toggleListItemCheckBox($event)" v-bind:id="availableItem.id" />
                <label>{{ availableItem.name }}</label>
            </li>
        </ul>
    </div>
</template>

<script>
    export default {
        props: {
            title: {
                type: String,
                required: true
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
            }
        }
    };
</script>
