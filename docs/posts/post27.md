---
draft: false
date: 2025-03-07
categories:
  - git
---

# git goodies

```
git branch -d `git branch | grep feature`                             # delete all branches with feature in its name
git branch | grep feature | xargs git branch -d                       # same as ^
git push origin --delete branchXYZ                                    # git push origin :branchXYZ
git push origin --delete `git branch -r | grep feature | cut -c10-`   # delete all feature branches from remote
git show branch:file > write_here                                     # https://stackoverflow.com/questions/7856416/view-a-file-in-a-different-git-branch-without-changing-branches
git checkout branch -- path/to/file                                   # similar to previous but checkout the file
git checkout --ours foo/bar.java                                      # useful for rebases or merges, works with --theirs too
git log -p -- src/data_capture_tools                                  # changes only made to a specific directory
git log --all --full-history -- "**/thefile.*"                        # https://stackoverflow.com/questions/7203515/how-to-find-a-deleted-file-in-the-project-commit-history
git log --left-right --graph --cherry-pick --oneline feature...branch # https://til.hashrocket.com/posts/18139f4f20-list-different-commits-between-two-branches
```
