<template>
    <div class="media">
        <img class="mr-3 comments-img" src="http://placekitten.com/100/100" alt="Generic placeholder image">
        <div class="media-body">
            <h6 class="media-heading">{{firstName + " " + lastName}}<small><i> {{GetDateFromNow(dateTime)}}</i></small></h6>
            <div class="media-comment">{{ commentBody }}</div>
            <div class="media-buttons">
                <a href="javascript:;" class="badge badge-primary-light"><i class="far fa-thumbs-up"></i> 32</a>
                <a href="javascript:;" class="badge badge-primary-light open-comments-btn" v-on:click="EmitOpenCommentsModal(reqIndex, reqId, id, commentBody, firstName, lastName, userId)"><i class="fas fa-reply"></i> Reply</a>
                <a href="javascript:;" class="badge badge-danger-light delete-comments-btn" v-on:click="EmitDeleteComment(reqIndex, commentIndex, id)" v-if="currentUser == userId"><i class="fas fa-trash-alt"></i> Delete</a>
                <a href="javascript:;" class="badge badge-secondary-light delete-comments-btn" v-on:click="ShowReplies(mainId)" data-toggle="collapse" :data-target="`#children-${mainId}`" v-if="parentId == null && children.length"><i :id="`replies-chevron-${mainId}`" class="fas fa-chevron-circle-down rotate"></i> See Replies</a>
            </div>

            <div :id="`children-${mainId}`" class="row collapse">
                <div>
                    <div>
                        <tree-comments v-if="children.length && !maxDepthReached"
                                       v-for="child in children"
                                       :id="child.id"
                                       :main-id="mainId"
                                       :req-id="reqId"
                                       :req-index="reqIndex"
                                       :parent-id="child.parentId"
                                       :comment-index="commentIndex"
                                       :user-id="child.userId"
                                       :current-user="child.currentUser"
                                       :first-name="child.firstName"
                                       :last-name="child.lastName"
                                       :date-time="child.dateTime"
                                       :children="child.children || []"
                                       :comment-body="child.commentBody"
                                       :depth="depth + 1" />
                    </div>
                </div>


                <hr v-if="parentId == null" />
            </div>
        </div>
</template>

<script>
    const MAX_DEPTH = 8;

    export default {
        name: 'TreeComments',
        props: {
            id: Number,
            mainId: Number,
            reqId: Number,
            reqIndex: Number,
            parentId: Number,
            commentIndex: Number,
            userId: String,
            currentUser: String,
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
            ShowReplies: function (mainId) {

                const chevron = "#replies-chevron-" + mainId;

                console.log(this.projectModel);
                $(chevron).toggleClass("down");

            },
            EmitOpenCommentsModal: function (reqIndex, reqId, parentId, commentBody, parentFName, parentLName, userId) {

                var payload = {
                    reqIndex: this.reqIndex,
                    reqId: this.reqId,
                    parentId: this.id,
                    commentBody: this.commentBody,
                    parentFName: this.firstName,
                    parentLName: this.lastName,
                    userId: this.userId
                }

                this.$root.$emit('CommentsModal', payload);

            },

            EmitDeleteComment(reqIndex, reqId, id) {

                var payload = {
                    reqIndex: this.reqIndex,
                    commentIndex: this.commentIndex,
                    id: this.id
                }

                this.$root.$emit('DeleteComment', payload);

            },
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