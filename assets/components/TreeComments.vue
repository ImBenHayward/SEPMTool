<template>
    <div class="media">
        <img class="mr-3 comments-img" src="http://placekitten.com/100/100" alt="Generic placeholder image">
        <div class="media-body">
            <h6 class="media-heading">{{firstName + " " + lastName}}<small><i> {{GetDateFromNow(dateTime)}}</i></small></h6>
            <div class="media-comment">{{ commentBody }}</div>
            {{currentUser}}
            {{userId}}
            <div class="media-buttons">
                <a href="#" class="badge badge-primary-light"><i class="far fa-thumbs-up"></i> 32</a>
                <a href="#" class="badge badge-primary-light" v-on:click="OpenCommentsModal2(reqId, id, commentBody, firstName, lastName)"><i class="fas fa-reply"></i> Reply</a>
                <a href="#" class="badge badge-danger-light" v-on:click="OpenCommentsModal2(reqId, id, commentBody, firstName, lastName)" v-if="currentUser == userId"><i class="fas fa-reply"></i> Delete</a>
            </div>
            <!--<div ><i class="fas fa-reply comments-reply"></i> Reply</div>-->

            <tree-comments v-if="children.length && !maxDepthReached"
                           v-for="child in children"
                           :id="child.id"
                           :req-id="reqId"
                           :comment-user="child.userId"
                           :current-user="child.currentUser"
                           :first-name="child.firstName"
                           :last-name="child.lastName"
                           :date-time="child.dateTime"
                           :children="child.children || []"
                           :comment-body="child.commentBody"
                           :depth="depth + 1" />
        </div>
    </div>
    
</template>

<script>
    const MAX_DEPTH = 99;

    export default {
        name: 'TreeComments',
        props: {
            id: Number,
            reqId: Number,
            commentBody: String,
            firstName: String,
            lastName: String,
            dateTime: null,
            children: Array,
            depth: {
                type: Number,
                default: 1,
            },
        },
        methods: {
            OpenCommentsModal2: function (reqId, parentId, commentBody, parentFName, parentLName) {

                var payload = {
                    reqId: this.reqId,
                    parentId: this.id,
                    commentBody: this.commentBody,
                    parentFName: this.firstName,
                    parentLName: this.lastName
                }

                this.$root.$emit('component1', payload);

                //this.commentModel.requirementId = reqId;
                //this.commentModel.parentId = parentId;
                //this.commentModel.parentComment = commentBody;
                //this.commentModel.parentFName = parentFName;
                //this.commentModel.parentLName = parentLName;

            },
            //moment(...args) {
            //    return moment(...args);
            //},
            GetDateFromNow(date) {
                return this.$moment(date).fromNow();
            },
        },
        computed: {
            maxDepthReached() {
                return this.depth >= MAX_DEPTH;
            }
        }
    };
</script>

<!--<template>
    <div class="tree-menu">
        <div>hello</div>
        <tree-menu v-for="node in nodes"
                   :nodes="node.nodes"
                   :label="node.label">
        </tree-menu>
    </div>
</template>

<script>
  export default {
    props: [ 'label', 'nodes' ],
    name: 'child-component'
  }
</script>-->
