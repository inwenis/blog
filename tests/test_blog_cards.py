import tempfile
import textwrap
import unittest
from pathlib import Path

from mkdocs.commands.build import build
from mkdocs.config import load_config


ROOT = Path(__file__).resolve().parents[1]


class BlogCardTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.temp_dir = tempfile.TemporaryDirectory()
        project = Path(cls.temp_dir.name)
        docs = project / "docs"
        posts = docs / "posts"
        posts.mkdir(parents=True)

        (docs / "index.md").write_text("# Test blog\n", encoding="utf-8")
        (posts / "unsafe.md").write_text(
            textwrap.dedent(
                """\
                ---
                date: 2026-01-02
                categories:
                  - Security
                ---

                # Unsafe sample

                ```html
                <img src=x onerror=alert(1)>
                ```
                """
            ),
            encoding="utf-8",
        )
        (posts / "draft.md").write_text(
            textwrap.dedent(
                """\
                ---
                date: 2026-01-01
                draft: true
                pin: true
                categories:
                  - Preview
                ---

                # Draft sample

                Draft content.
                """
            ),
            encoding="utf-8",
        )

        config_path = project / "mkdocs.yml"
        config_path.write_text(
            textwrap.dedent(
                f"""\
                site_name: Test blog
                theme:
                  name: material
                  custom_dir: "{(ROOT / 'overrides').as_posix()}"
                plugins:
                  - blog:
                      blog_dir: .
                      draft: true
                  - search
                markdown_extensions:
                  - pymdownx.superfences
                nav:
                  - index.md
                """
            ),
            encoding="utf-8",
        )

        config = load_config(config_file=str(config_path))
        build(config)
        cls.html = (project / "site" / "index.html").read_text(encoding="utf-8")

    @classmethod
    def tearDownClass(cls):
        cls.temp_dir.cleanup()

    def test_summary_escapes_code_that_looks_like_html(self):
        self.assertIn("&lt;img src=x onerror=alert(1)&gt;", self.html)
        self.assertNotIn("<img src=x onerror=alert(1)>", self.html)

    def test_draft_and_pin_statuses_are_visible(self):
        self.assertIn("blog-card__status--pinned", self.html)
        self.assertIn("blog-card__status--draft", self.html)
        self.assertIn("Draft", self.html)

    def test_categories_use_lists_instead_of_navigation_landmarks(self):
        self.assertIn('<ul class="blog-card__categories"', self.html)
        self.assertNotIn('<nav class="blog-card__categories"', self.html)


if __name__ == "__main__":
    unittest.main()
