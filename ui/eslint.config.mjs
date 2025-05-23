import pluginJs from '@eslint/js';
import globals from 'globals';
import tseslint from 'typescript-eslint';

export default [
  {
    ignores: ['sketches/'],
  },
  {
    files: ['**/*.{js,mjs,cjs,ts}'],
  },
  {
    languageOptions: { globals: globals.browser },
  },
  pluginJs.configs.recommended,
  ...tseslint.configs.recommended,
];
